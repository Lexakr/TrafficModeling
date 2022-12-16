using System;

namespace TrafficModeling.Model
{
    /// <summary>
    /// Статический класс (библиотека) генераторов псевдо-СВ
    /// </summary>
    internal static class Randoms
    {
        /// <summary>
        /// Функция генерации экспоненциально распределенной псевдослучайной величины
        /// с заданными параметрами.
        /// </summary>
        /// <param name="a">Нижняя граница интервала</param>
        /// <param name="b">Верхняя граница интервала</param>
        /// <param name="lambda">Лямбда</param>
        /// <param name="round">Округление</param>
        /// <returns>Экспоненциальная псевдо-СВ double.</returns>
        public static double GetExpNum(int a, int b, double lambda)
        {
            var rand = new Random(); // Псевдослучайный ГЧ
            var exp_rate = Math.Exp(-lambda * a); // Параметр скорости (лямбда)
            return // Экспоненциальная СВ на (a, b), округленная до 0,1
                Math.Round((-Math.Log(exp_rate - rand.NextDouble() * (exp_rate - Math.Exp(-lambda * b))) / lambda), 1); 
        }

        /// <summary>
        /// Функция генерации нормально распределенной псевдослучайной величины
        /// с заданными параметрами.
        /// </summary>
        /// <param name="mean">Мат. ожидание</param>
        /// <param name="stdDev">Отклонение</param>
        /// <param name="round">Округление</param>
        /// <returns>Нормальная псевдо-СВ double.</returns>
        public static double GetNormalNum(double mean, double stdDev)
        {
            Random rand = new(); // Псевдослучайный ГЧ
            double u1 = 1.0 - rand.NextDouble(); // равномерные СВ на (0,1]
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2); // нормальная СВ на (0,1)  
            return // нормальная СВ (мат.ожид, дисперсия), округленная до 0,1
                Math.Round((mean + stdDev * randStdNormal), 1); 
        }

        /// <summary>
        /// Функция генерации дискретной всевдо-СВ по закону Пуассона
        /// с заданным параметром.
        /// </summary>
        /// <param name="lambda">Лямбда</param>
        /// <returns>Пуассоновская псевдо-СВ int.</returns>
        public static int GetPoissonNum(double lambda)
        {
            // Алгоритм Дональда Кнута, 1969.
            if (lambda > 30) return -1; // TODO: Неэффективно для больших лямбд (программное ограничение, вынести в UI)
            Random r = new();
            double p = 1.0, L = Math.Exp(-lambda);
            int k = 0;
            do
            {
                k++;
                p *= r.NextDouble();
            } while (p > L);
            return k - 1;
        }
    }
}
