namespace TrafficModeling.Model
{
    /// <summary>
    /// Класс, генерирующий объекты Car.
    /// </summary>
    internal class CarGenerator
    {
        private static int carCounter = 0;

        /// <summary>
        /// Мат ожидание скорости для Cars.
        /// </summary>
        public double CarSpeedExpValue { get; set; }
        /// <summary>
        /// Дисперсия скорости для Cars.
        /// </summary>
        public double CarSpeedDispersion { get; set; }

        public CarGenerator(double carSpeedExpValue, double carSpeedDispersion)
        {
            CarSpeedExpValue = carSpeedExpValue;
            CarSpeedDispersion = carSpeedDispersion;
        }

        /// <summary>
        /// Функция, возвращающая созданный объект Car со случайными характеристиками, имеющий уникальное имя.
        /// </summary>
        /// <param name="origin">Поток, в котором генерируется Car</param>
        /// <param name="time">Текущее время симуляции</param>
        /// <returns></returns>
        public Car Generate(string origin, int time)
        {
            carCounter++;
            return new Car("Car_" + carCounter, (int)Randoms.GetNormalNum(CarSpeedExpValue, CarSpeedDispersion), origin, time);
        }
    }
}
