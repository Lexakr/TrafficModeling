using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace TrafficModeling.Model
{
    class Statistics
    {
        public List<int> TotalCars { get; set; }
        public List<int> TotalCarsByHour { get; set; }
        public List<int> AverageWaitingTime { get; set; }
        public List<int> AverageServeTime { get; set; }
        public List<int> CarsInQueDynamics { get; set; }

        public Statistics()
        {
            TotalCars = new()
            {
                0
            };
        }

        public void Add(ServeStream stream, int time)
        {
            if(stream.ServedCars != null)
                TotalCars.Add(stream.ServedCars.Count);
            if(time % 36000 == 0)
            {

            }
        }

        public Statistics GetByMinuts(Statistics stat)
        {
            // Объединение данных из посекундных в поминутные
            return stat;
        }
    }
}
