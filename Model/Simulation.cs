using Microsoft.VisualBasic;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulationTime"></param>
        /// <param name="u1"></param>
        /// <param name="stdDev1"></param>
        /// <param name="stdDev2"></param>
        /// <param name="u2"></param>
        /// <param name="trafficLightTime"></param>
        /// <param name="delay"></param>
        /// <param name="length"></param>
        public Simulation(int simulationTime = 432000, double u1 = 10, double stdDev1 = 2, double u2 = 8,
            double stdDev2 = 3, int trafficLightTime = 34, int delay = 90, int length = 500)
        {
            this.simulationTime = simulationTime; // 12 часов
            time = new();
            serveStream = new(u1, stdDev1, u2, stdDev2, trafficLightTime, delay, length); // lambda1, lambda 2, light_time, stream_length, delay_time
            simStats = new();
            // подписываем обсерверов
            time.Attach(serveStream);
            time.Attach(serveStream.InStream1);
            time.Attach(serveStream.InStream2);
        }

/*        public Simulation(int simulationTime, double u1, double stdDev1, double u2,
            double stdDev2, int trafficLightTime, int delay, int length)
        {
            this.simulationTime = simulationTime;
            time = new();
            serveStream = new(u1, stdDev1, u2, stdDev2, trafficLightTime, delay, length);
            // подписываем обсерверов
            time.Attach(serveStream);
            time.Attach(serveStream.InStream1);
            time.Attach(serveStream.InStream2);
        }*/

        /// <summary>
        /// 
        /// </summary>
        public void Run()
        {
            // симуляция заданного количества времени
            for (int i = 1; i < simulationTime; i++)
            {
                if(i % 600 == 0)
                    simStats.Add(serveStream, time.CurrentTime);
                time.Increment();
            }
        }
    }
}
