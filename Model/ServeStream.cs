using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;

namespace TrafficModeling.Model
{
    internal class ServeStream : ITimeObserver
    {
        private InputStream inputStream1;
        private InputStream inputStream2;
        private TrafficLight tLight1;
        private TrafficLight tLight2;

        private Queue<Car> serveQueue;
        private int length;
        private int addCounter;
        private int nextAddRequestTime;

        private readonly Random r; // UTILITY

        public List<Car> ServedCars { get; set; }

        public InputStream InStream1 { get => inputStream1; }
        public InputStream InStream2 { get => inputStream2; }
        public TrafficLight TLight1 { get => tLight1; }
        public TrafficLight TLight2 { get => tLight2; }

        public ServeStream(double u1, double stdDev1, double u2, double stdDev2, int trafficLightTime, int delay, int length)
        {
            this.length = length; // длина участка под светофорами в метрах
            ServedCars = new();
            inputStream1 = new(u1, stdDev1, "Input Stream 1");
            inputStream2 = new(u2, stdDev2, "Input Stream 2");
            tLight1 = new(trafficLightTime * 10, delay * 10, true);
            tLight2 = new(trafficLightTime * 10, delay * 10, false);

            serveQueue = new();
            this.length = length;
            addCounter = 0;
            r = new();
            nextAddRequestTime = NextAddRequestTime(); // (1.0, 2.0)
        }

        public void Update(ITime time)
        {
            Serve();     // обслуживание заявок
            ServeLights();
            AddRequest(); // прием заявок на обслуживание
        }

        /// <summary>
        /// Обработка заявок в очереди обслуживания.
        /// </summary>
        public void Serve()
        {
            if (serveQueue.Count == 0) 
                return; // Нечего обрабатывать
            var flag = false;
            // Проходим все машины на участке дороги, декрементируя каждый тик оставшееся время до конца участка
            foreach(Car c in serveQueue)
            {
                c.TravelTimeLeft--;
                // машина достигла конца участка, эта заявка обслужена
                if (c.TravelTimeLeft == 0)
                    flag = true;
            }
            if(flag)
                ServedCars.Add(serveQueue.Dequeue());
        }

        /// <summary>
        /// Добавление заявок в очередь обслуживание.
        /// </summary>
        public void AddRequest()
        { 
            // Заявки добавляются только когда один из светофоров зеленый
            if(tLight1.IsGreen || tLight2.IsGreen)
            {
                addCounter++;

                if(addCounter == nextAddRequestTime)
                {
                    if ((tLight1.Counter == 0 || tLight2.Counter == 0) && serveQueue.Count != 0)
                        throw new Exception("Ex");

                    nextAddRequestTime = NextAddRequestTime(); // новое время до генерации заявки
                    addCounter = 0; // обнуление счетчика
                    if (tLight1.IsGreen) AddRequestFrom(inputStream1);
                    else if (tLight2.IsGreen) AddRequestFrom(inputStream2);
                }
            }
        }

        public void AddRequestFrom(InputStream stream)
        {
            // Не обслуживаем, если в очереди нет заявок
            if (stream.InputQueue.Count == 0) 
                return;

            var car = stream.InputQueue.Dequeue(); // (int)speed * 1000 / 60 / 60 / 10

            car.TravelTimeLeft = (int)((double)length / ((double)car.Speed / 36.0)); // задаем время проезда участка

            // Если впереди есть машина, которой ехать дольше (упремся в нее), то время движения берется ее + 1с интервал
            if (serveQueue.Count != 0 && serveQueue.Last().TravelTimeLeft >= car.TravelTimeLeft)
                car.TravelTimeLeft = serveQueue.Last().TravelTimeLeft + NextAddRequestTime();

            car.TravelTime = car.TravelTimeLeft;
            serveQueue.Enqueue(car);
        }

        public int NextAddRequestTime()
        {
            var res = (int)(Randoms.GetNormalNum(1.5, 0.1) * 10);
            if (res < 0) 
                res *= -1;
            return res;
        }

        public void ServeLights()
        {
            tLight1.Update();
            tLight2.Update();
        }
    }
}
