using System;
using System.Collections.Generic;
using System.Linq;

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

        public ServeStream(InputStream inputStream1, InputStream inputStream2, int trafficLightTime, int delay, int length)
        {
            this.length = length; // длина участка под светофорами в метрах
            ServedCars = new();
            this.inputStream1 = inputStream1;
            this.inputStream2 = inputStream2;
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
            Serve(time.CurrentTime);     // обслуживание заявок
            ServeLights();
            AddRequest(time.CurrentTime); // прием заявок на обслуживание
        }

        /// <summary>
        /// Обработка заявок в очереди обслуживания.
        /// </summary>
        public void Serve(int time)
        {
            if (serveQueue.Count == 0)
                return; // Нечего обрабатывать
            var flag = false;
            // Проходим все машины на участке дороги, декрементируя каждый тик оставшееся время до конца участка
            foreach (Car c in serveQueue)
            {
                c.TravelTimeLeft--;
                // машина достигла конца участка, эта заявка обслужена
                if (c.TravelTimeLeft == 0)
                {
                    c.DepartureTime = time; // отметка о времени покидания участка
                    flag = true;
                }

            }
            if (flag)
                ServedCars.Add(serveQueue.Dequeue());
        }

        /// <summary>
        /// Добавление заявок в очередь обслуживание.
        /// </summary>
        public void AddRequest(int time)
        {
            // Заявки добавляются только когда один из светофоров зеленый
            if (tLight1.IsGreen || tLight2.IsGreen)
            {
                addCounter++;

                if (addCounter == nextAddRequestTime)
                {
                    if ((tLight1.Counter == 0 || tLight2.Counter == 0) && serveQueue.Count > 0)
                        throw new Exception("A collision has occurred! Try setting a longer traffic light delay. " + this.serveQueue.Last().TravelTime);

                    nextAddRequestTime = NextAddRequestTime(); // новое время до генерации заявки
                    addCounter = 0; // обнуление счетчика
                    if (tLight1.IsGreen)
                        AddRequestFrom(inputStream1, time);
                    else if (tLight2.IsGreen)
                        AddRequestFrom(inputStream2, time);
                }
            }
        }

        public void AddRequestFrom(InputStream stream, int time)
        {
            // Не обслуживаем, если в очереди нет заявок
            if (stream.InputQueue.Count == 0)
                return;

            var car = stream.InputQueue.First(); // (int)speed * 1000 / 60 / 60 / 10
            stream.InputQueue.RemoveAt(0); 

            car.TravelTimeLeft = (int)((double)length / ((double)car.Speed / 36.0)); // задаем время проезда участка

            // Если впереди есть машина, которой ехать дольше (упремся в нее), то время движения берется ее + 1с интервал
            if (serveQueue.Count != 0 && serveQueue.Last().TravelTimeLeft >= car.TravelTimeLeft)
                car.TravelTimeLeft = serveQueue.Last().TravelTimeLeft + NextAddRequestTime();

            car.WaitingTime = time - car.ArrivalTime;

            car.TravelTime = car.TravelTimeLeft;
            serveQueue.Enqueue(car);
        }

        public int NextAddRequestTime()
        {
            var result = (int)(Randoms.GetNormalNum(1.8, 0.3) * 10);
            if (result < 0)
                result *= -1;
            if (result == 0)
                result = 1;
            return result;
        }

        public void ServeLights()
        {
            tLight1.Update();
            tLight2.Update();
        }
    }
}
