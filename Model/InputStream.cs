using System;
using System.Collections.Generic;

namespace TrafficModeling.Model
{
    /// <summary>
    /// Входной поток машин (очередь). Основная функция - генерация экземпляров автомобилей. Реализует паттерн Наблюдатель
    /// </summary>
    internal class InputStream : ITimeObserver
    {
        /// <summary>
        /// Экземпляр генератора автомобилей
        /// </summary>
        private readonly CarGenerator carGen;

        /// <summary>
        /// Мат ожидание (через сколько тиков создать новый Car)
        /// </summary>
        private readonly double expectedValue;

        /// <summary>
        /// Имя входящего потока
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Дисперсия 
        /// </summary>
        private readonly double stdDev;

        /// <summary>
        /// Счетчик времени в тиках для генерации машины
        /// </summary>
        private int timeCounter = 0;

        /// <summary>
        /// Время в тиках, через сколько возникнет событие генерации машины
        /// </summary>
        private int carGenTime;

        /// <summary>
        /// Очередь машин входящего потока
        /// </summary>
        public List<Car> InputQue { get; set; }

        public InputStream(CarGenerator carGen, double expectedValue, double stdDev, string streamName)
        {
            this.carGen = carGen;
            InputQue = new();
            this.expectedValue = expectedValue;
            this.stdDev = stdDev;
            carGenTime = NextCarGenTime(); // время первой генерации
            name = streamName;
        }

        /// <summary>
        /// Генерация времени в тиках до следующего появления машины.
        /// </summary>
        /// <returns>Тиков до следующей генерации машины</returns>
        public int NextCarGenTime()
        {
            int result = (int)(Randoms.GetNormalNum(expectedValue, stdDev) * 10);

            // Следим, чтобы случайная величина всегда была > 0
            if (result < 0)
                result *= -1;
            if (result == 0)
                result = 1;

            return result;
        }

        /// <summary>
        /// Реализация Update паттерна Наблюдатель. Генерация новой машины в очередь в заданное случайное время.
        /// </summary>
        /// <param name="time">Текущее время симуляции в тиках</param>
        /// <exception cref="Exception"></exception>
        public void Update(ITime timer)
        {
            AddCar(timer.CurrentTime);
        }

        public void AddCar(int time)
        {
            timeCounter++;

            if (timeCounter == carGenTime)
            {
                var car = carGen.Generate(this.name, time);

                // Машины спецслужь помещаются на первое место в очереди
                if (car.Priority)
                    InputQue.Insert(0, car);
                else
                    InputQue.Add(car);

                timeCounter = 0;
                // время до следующей генерации
                carGenTime = NextCarGenTime();
            }
        }
    }
}
