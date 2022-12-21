using System;
using System.Collections.Generic;
using System.Linq;

namespace TrafficModeling.Model
{
    /// <summary>
    /// Обслуживающий поток Q-схемы, включает в себя 2 входящих потока с клапанами (светофорами). Реализует интерфейс Наблюдатель.
    /// </summary>
    internal class ServeStream : ITimeObserver
    {
        /// <summary>
        /// Первый входящий поток
        /// </summary>
        private readonly InputStream inputStream1;
        
        /// <summary>
        /// Второй входящий поток
        /// </summary>
        private readonly InputStream inputStream2;

        /// <summary>
        /// Светофор первого потока
        /// </summary>
        private readonly TrafficLight tLight1;

        /// <summary>
        /// Светофор второго потока
        /// </summary>
        private readonly TrafficLight tLight2;

        /// <summary>
        /// Поток обслуживания (очередь)
        /// </summary>
        private readonly Queue<Car> serveQueue;

        /// <summary>
        /// Длина участка (потока обслуживания)
        /// </summary>
        private readonly int length;

        /// <summary>
        /// Счетчик добавления машин из входящих потоков в обслуживающий
        /// </summary>
        private int addCounter;

        /// <summary>
        /// Время в тиках до следующего добавления машины из входяшего потока
        /// </summary>
        private int nextAddRequestTime;

        /// <summary>
        /// Обслуженные потоком машины. Для статистики
        /// </summary>
        public List<Car> ServedCars { get; set; }

        /// <summary>
        /// Первый входящий поток
        /// </summary>
        public InputStream InStream1 { get => inputStream1; }
        /// <summary>
        /// Второй входящий поток
        /// </summary>
        public InputStream InStream2 { get => inputStream2; }
        /// <summary>
        /// Светофор первого потока
        /// </summary>
        public TrafficLight TLight1 { get => tLight1; }
        /// <summary>
        /// Светофор второго потока
        /// </summary>
        public TrafficLight TLight2 { get => tLight2; }

        public ServeStream(InputStream inputStream1, InputStream inputStream2, int trafficLightTime, int delay, int length)
        {
            this.length = length; // длина участка под светофорами в метрах
            this.inputStream1 = inputStream1;
            this.inputStream2 = inputStream2;

            // Конвертируем время из секунд в децисекунды
            tLight1 = new(trafficLightTime * 10, delay * 10, true);
            tLight2 = new(trafficLightTime * 10, delay * 10, false);

            serveQueue = new();
            ServedCars = new();
            addCounter = 0;
            // Время до первого добавления автомобиля в поток обслуживания
            nextAddRequestTime = NextAddRequestTime(); 
        }

        /// <summary>
        /// Реализация Update паттерна Наблюдатель. Обслуживание потока и заявок.
        /// </summary>
        /// <param name="time">Время симуляции в тиках</param>
        public void Update(ITime time)
        {
            Serve();
            AddRequest(time.CurrentTime); // прием заявок на обслуживание
        }

        /// <summary>
        /// Обработка заявок в потоке обслуживания.
        /// </summary>
        public void Serve()
        {
            // Пустой поток
            if (serveQueue.Count == 0)
                return;

            var flag = false;

            // Проходим все машины на участке дороги, декрементируя каждый тик оставшееся время до конца участка
            foreach (Car c in serveQueue)
            {
                c.TravelTimeLeft--;
                // Машина достигла конца участка, эта заявка, находящаяся в начале очереди, обслужена и ее надо извлечь
                if (c.TravelTimeLeft == 0)
                {
                    flag = true;
                }
            }

            // Удаляем машину из очереди
            if (flag)
                ServedCars.Add(serveQueue.Dequeue());
        }

        /// <summary>
        /// Добавление заявок в поток обслуживания.
        /// </summary>
        /// <param name="time">Текущее время в тиках</param>
        public void AddRequest(int time)
        {
            // Заявки добавляются только когда один из светофоров зеленый
            if (tLight1.IsGreen || tLight2.IsGreen)
            {
                addCounter++;

                if (addCounter == nextAddRequestTime)
                {
                    // Ситуация столкновения машин, двигающихся навстречу друг другу (параметры симуляции нужно изменить).
                    if ((tLight1.TimeCounter == 0 || tLight2.TimeCounter == 0) && serveQueue.Count > 0)
                        throw new Exception("A collision has occurred! Try setting a longer traffic light delay.");

                    nextAddRequestTime = NextAddRequestTime(); // новое время до генерации заявки
                    addCounter = 0;

                    // Добавляем в поток обслуживания машину в зависимости от того, какой светофор зеленый
                    if (tLight1.IsGreen)
                        AddCarFromInputStream(inputStream1, time);
                    else if (tLight2.IsGreen)
                        AddCarFromInputStream(inputStream2, time);
                }
            }
        }

        /// <summary>
        /// Удаление машины из начала очереди входного потока и добавление в очередь потока обслуживания.
        /// </summary>
        /// <param name="stream">Входной поток</param>
        /// <param name="time">Текущее время в тиках</param>
        public void AddCarFromInputStream(InputStream stream, int time)
        {
            // Нет заявок в очереди
            if (stream.InputQue.Count == 0)
                return;

            var car = stream.InputQue.First();
            stream.InputQue.RemoveAt(0);

            // Время проезда участка = длина / скорость (в м/дс)
            car.TravelTimeLeft = (int)((double)length / ((double)car.Speed / 36.0));

            // Если впереди есть машина, которой ехать дольше (упремся в нее), то время проезда берется ее + случайный интервал интервал
            if (serveQueue.Count != 0 && serveQueue.Last().TravelTimeLeft >= car.TravelTimeLeft)
                car.TravelTimeLeft = serveQueue.Last().TravelTimeLeft + NextAddRequestTime();

            car.WaitingTime = time - car.ArrivalTime;
            car.TravelTime = car.TravelTimeLeft;

            serveQueue.Enqueue(car);
        }

        /// <summary>
        /// Время в тиках до следующего события. Нормальное распределение с expValue = 1.8, dispersion = 0.3
        /// </summary>
        /// <returns></returns>
        public int NextAddRequestTime()
        {
            var result = (int)(Randoms.GetNormalNum(1.8, 0.3) * 10);

            // Следим, чтобы случайная величина всегда была > 0
            if (result < 0)
                result *= -1;
            if (result == 0)
                result = 1;

            return result;
        }
    }
}
