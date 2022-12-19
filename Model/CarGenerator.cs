using System;
using MathNet.Numerics.Distributions;

namespace TrafficModeling.Model
{
    internal class CarGenerator
    {
        private static Bernoulli bernoulli = new(0.99); // 1% шанс появления машины спецслужб
        private static int civCarCounter = 0;
        private static int govCarCounter = 0;

        /// <summary>
        /// Мат ожидание скорости для Civil Cars.
        /// </summary>
        public double CivilExpValue { get; set; }
        /// <summary>
        /// Дисперсия скорости для Civil Cars.
        /// </summary>
        public double CivilDispersion { get; set; }
        /// <summary>
        /// Мат ожидание скорости для Goverment Cars.
        /// </summary>
        public double GovExpValue { get; set; }
        /// <summary>
        /// Дисперсия скорости для Goverment Cars.
        /// </summary>
        public double GovDispersion { get; set; }

        public CarGenerator(double civilExpValue, double civilDispersion, double govExpValue, double govDispersion)
        {
            CivilExpValue = civilExpValue;
            CivilDispersion = civilDispersion;
            GovExpValue = govExpValue;
            GovDispersion = govDispersion;
        }

        public Car Generate(string origin, ITime time)
        {
            int result = bernoulli.Sample();

            if (result == 1)
            {
                civCarCounter++;
                return new CivilCar("Civil_Car_" + civCarCounter, (int)Randoms.GetNormalNum(CivilExpValue, CivilDispersion), origin, time.CurrentTime);
            }
            else
            {
                govCarCounter++;
                return new GovCar("Goverment_Car_" + govCarCounter, (int)Randoms.GetNormalNum(GovExpValue, GovDispersion), origin, time.CurrentTime);
            }
        }
    }
}

