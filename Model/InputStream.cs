using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TrafficModeling.Model
{
    internal class InputStream : ITimeObserver
    {
        private CarGenerator carGen; // как лучше?????
        private List<Car> inputQue;
        private double u; // Мат ожидание (для выбора через сколько сек создать новую машину)
        private double stdDev; // dispersin
        private int counter = 0; // счетчик времени
        private int carGenTime; // время возникновения события

        public string Name { get; set; }
        public List<Car> InputQueue { get => inputQue; set => inputQue = value; }

        public InputStream(CarGenerator carGen, double u, double stdDev, string streamName)
        {
            this.carGen = carGen;
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
            counter++;
            if (counter == carGenTime)
            {
                var car = carGen.Generate(this.Name, time);
                if (car.Priority)
                    inputQue.Insert(0, car); // Вне очереди
                else inputQue.Add(car); // создаем авто и помещаем в очередь
                if (inputQue.Count > 1000000)
                    throw new Exception(Name + " reached the number of 10000 vehicles. Further simulation is pointless." +
                        " Change the simulation parameters for this flow or for the traffic light.");
                counter = 0;
                carGenTime = NextCarGenTime(); // время до следующей генерации\
            }
        }
    }
}
