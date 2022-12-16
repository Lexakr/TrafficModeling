using System;
using MathNet.Numerics.Distributions;

namespace TrafficModeling.Model
{
    internal class CarGenerator
    {
        private static Bernoulli b = new(0.99); // 1% шанс появления машины спецслужб
        private static int civCarCounter = 0;
        private static int govCarCounter = 0;

        // TODO: временный конструктор для отладки, удалить при финальной версии
        public Car Generate(string origin)
        {
            int c = b.Sample();

            if (c == 1)
            {
                civCarCounter++;
                return new CivilCar("Civil_Car_" + civCarCounter, (int)Randoms.GetNormalNum(75, 1), origin);
            }
            else
            {
                govCarCounter++;
                return new CivilCar("Goverment_Car_" + govCarCounter, (int)Randoms.GetExpNum(85, 100, 1), origin);
            }
        }

        // TODO: Конструктор с параметрами, заданными юзером (мин-макс скорость, параметры генерации и тд)
    }
}

