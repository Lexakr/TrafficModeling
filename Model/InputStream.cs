using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TrafficModeling.Model
{
    internal class InputStream : ITimeObserver
    {
        private CarGenerator carGen; // как лучше?????
        private Queue<Car> inputQue;
        private double u; // Мат ожидание (для выбора через сколько сек создать новую машину)
        private double stdDev; // dispersin
        private int counter = 0; // счетчик времени
        private int carGenTime; // время возникновения события

        public string Name { get; set; }
        public Queue<Car> InputQueue { get => inputQue; set => inputQue = value; }

        public InputStream(double u, double stdDev, string streamName)
        {
            carGen = new();
            inputQue = new();
            this.u = u;
            this.stdDev = stdDev;
            carGenTime = NextCarGenTime(); // время первой генерации
            Name = streamName;
        }

        /// <summary>
        /// Генерация времени до следующего появления машины.
        /// </summary>
        /// <returns></returns>
        public int NextCarGenTime()
        {
            int result = (int)(Randoms.GetNormalNum(u, stdDev) * 10);
            if (result < 0) 
                result *= -1;
            if (result == 0) 
                result = 1;
            return result;
        }

        public void Update(ITime time)
        {
            foreach (Car car in inputQue)
            {
                car.WaitingTime++;
            }

            counter++;
            if (counter == carGenTime)
            {
                var car = carGen.Generate(this.Name);
                if (car.Priority) inputQue.Append(car); // Вне очереди
                else inputQue.Enqueue(car); // создаем авто и помещаем в очередь
                counter = 0;
                carGenTime = NextCarGenTime(); // время до следующей генерации
                // TODO: Логгирование: Log.Add(string); Log.Clear(); singleton; MVP?
            }
        }
    }
}
