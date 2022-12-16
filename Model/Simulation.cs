namespace TrafficModeling.Model
{
    /// <summary>
    /// VIEV MODEL
    /// </summary>
    internal class Simulation
    {
        private int simulationTime;
        private Time time;
        private ServeStream serveStream;
        public Statistics simStats;

        public Simulation()
        {
            simulationTime = 360000; // 10 часов
            time = new();
            serveStream = new(10.0, 2.0, 8.0, 3.0, 34, 60, 500); // lambda1, lambda 2, light_time, stream_length, delay_time
            simStats = new();
            // подписываем обсерверов
            time.Attach(serveStream);
            time.Attach(serveStream.InStream1);
            time.Attach(serveStream.InStream2);
        }

        public Simulation(int simulationTime, double u1, double stdDev1, double stdDev2,
            double u2, int trafficLightTime, int delay, int length)
        {
            this.simulationTime = simulationTime;
            time = new();
            serveStream = new(u1, stdDev1, u2, stdDev2, trafficLightTime, delay, length);
            // подписываем обсерверов
            time.Attach(serveStream);
            time.Attach(serveStream.InStream1);
            time.Attach(serveStream.InStream2);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RunSimulation()
        {
            // симуляция заданного количества времени
            for (int i = 1; i <= simulationTime; i++)
            {
                if(i % 600 == 0)
                    simStats.Add(serveStream, time.CurrentTime);
                time.Increment();
            }
        }
    }
}
