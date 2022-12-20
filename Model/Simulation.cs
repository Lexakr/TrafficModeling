namespace TrafficModeling.Model
{
    /// <summary>
    /// VIEV MODEL
    /// </summary>
    internal class Simulation
    {
        private Timer timer;
        private ServeStream serveStream;
        public Statistics SimStats { get; set; }

        public int SimulationTime { get; set; }
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
        /// <param name="civilExpValue">Средняя скорость обычных машин</param>
        /// <param name="civilDispersion">Дисперсия скорости обычных машин</param>
        /// <param name="govExpValue">Средняя скорость машин спецслужб</param>
        /// <param name="govDispersion">Дисперсия скорости машин спецслужб</param>
        public Simulation(int simulationTime = 0, double inputStream1ExpValue = 0, double inputStream1Dispersion = 0, double inputStream2ExpValue = 0,
            double inputStream2Dispersion = 0, int lightTime = 0, int delayTime = 0, int roadLength = 0, double civilExpValue = 0,
            double civilDispersion = 0, double govExpValue = 0, double govDispersion = 0)
        {
            SimulationTime = simulationTime;
            timer = new();
            //serveStream = new(u1, stdDev1, u2, stdDev2, trafficLightTime, delay, length); // lambda1, lambda 2, light_time, stream_length, delay_time
            serveStream = new(
                new InputStream(new CarGenerator(civilExpValue, civilDispersion, govExpValue, govDispersion), inputStream1ExpValue, inputStream1Dispersion, "Input Stream 1"),
                new InputStream(new CarGenerator(civilExpValue, civilDispersion, govExpValue, govDispersion), inputStream2ExpValue, inputStream2Dispersion, "Input Stream 2"),
                lightTime, delayTime, roadLength);
            SimStats = new();
            // подписываем обсерверов
            timer.Attach(serveStream);
            timer.Attach(serveStream.InStream1);
            timer.Attach(serveStream.InStream2);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Run()
        {
            // симуляция заданного количества времени
            for (int i = 0; i < SimulationTime; i++)
            {
                if (i % 600 == 0)
                    SimStats.AddCarsInQueToStat(serveStream);
                timer.Increment();
            }
            // Записать статистику по автомобилям
            SimStats.Process(serveStream.ServedCars, serveStream.InStream1.InputQueue.Count + serveStream.InStream2.InputQueue.Count, 
                timer.CurrentTime);
        }
    }
}
