namespace TrafficModeling.Model
{
    /// <summary>
    /// Симуляция в Q-схеме - прибор, включающий в себя элементы Q-схемы.
    /// </summary>
    internal class Simulation
    {
        /// <summary>
        /// Таймер симуляции
        /// </summary>
        private readonly Timer timer;

        /// <summary>
        /// Обслуживающий поток. Включает 2 входных и 2 светофора
        /// </summary>
        private readonly ServeStream serveStream;

        public ServeStream ServeStream { get => serveStream; }

        /// <summary>
        /// Статистика по симуляции
        /// </summary>
        public Statistics SimStats { get; set; }

        /// <summary>
        /// Длительность симуляции
        /// </summary>
        public int SimulationTime { get; set; }

        /// <summary>
        /// Таймер симуляции
        /// </summary>
        public Timer Timer { get => timer; }

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        /// <param name="simulationTime">Время симуляции</param>
        /// <param name="inputStream1ExpValue">Среднее время появления машин в 1 потоке</param>
        /// <param name="inputStream1Dispersion">Дисперсия времени 1 потока</param>
        /// <param name="inputStream2ExpValue">Среднее время появления машин во 2 потоке</param>
        /// <param name="inputStream2Dispersion">Дисперсия времени 1 потока</param>
        /// <param name="lightTime">Длительность зеленой фазы светофора</param>
        /// <param name="delayTime">Длительность красной фазы светофора</param>
        /// <param name="roadLength">Длина участка симуляции</param>
        /// <param name="carSpeedExpValue">Средняя скорость обычных машин</param>
        /// <param name="carSpeedDispersion">Дисперсия скорости обычных машин</param>
        public Simulation(int simulationTime = 0, double inputStream1ExpValue = 0, double inputStream1Dispersion = 0, double inputStream2ExpValue = 0,
            double inputStream2Dispersion = 0, int lightTime = 0, int delayTime = 0, int roadLength = 0, double carSpeedExpValue = 0,
            double carSpeedDispersion = 0)
        {
            SimulationTime = simulationTime;
            timer = new();
            serveStream = new(
                new InputStream(new CarGenerator(carSpeedExpValue, carSpeedDispersion), inputStream1ExpValue, inputStream1Dispersion, "Input Stream 1"),
                new InputStream(new CarGenerator(carSpeedExpValue, carSpeedDispersion), inputStream2ExpValue, inputStream2Dispersion, "Input Stream 2"),
                lightTime, delayTime, roadLength);
            SimStats = new();

            // Подписываем наблюдателей. Светофоры обязательно первые, чтобы обслуживались первыми.
            timer.Attach(serveStream.TLight1);
            timer.Attach(serveStream.TLight2);

            timer.Attach(serveStream);
            timer.Attach(serveStream.InStream1);
            timer.Attach(serveStream.InStream2);
        }

        /// <summary>
        /// Запуск симуляции с заданным временем.
        /// </summary>
        public void Run()
        {
            for (int i = 0; i < SimulationTime; i++)
            {
                // Каждую минуту фиксируем количество машин в очередях входных потоков
                if (i % 600 == 0)
                    SimStats.AddCarsInQueToStat(serveStream);
                timer.Increment();
            }
            // Записываем статистику по автомобилям
            SimStats.Process(serveStream.ServedCars, serveStream.InStream1.InputQue.Count + serveStream.InStream2.InputQue.Count,
                timer.CurrentTime);
        }
    }
}
