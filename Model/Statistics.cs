using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace TrafficModeling.Model
{
    class Statistics
    {
        public int SimulationTime { get; set; }
        public int TotalCars { get; set; }
        public int TotalCivilCars { get; set; }
        public int TotalGovCars { get; set; }
        public int MaxTravelTime { get; set; } // машина которая прошла очередь быстрее всех
        public int MinTravelTime { get; set; }
        public int MaxWaitingTime { get; set; }
        public int TotalCarsInStream1 { get; set; }
        public int TotalCarsInStream2 { get; set; }
        public int AvgWaitingTimeInStream1 { get; set; }
        public int AvgWaitingTimeInStream2 { get; set; }
        public int AvgServeTimeInStream1 { get; set; }
        public int AvgServeTimeInStream2 { get; set; }
        public List<int> CarsInQue1Dynamics { get; set; }
        public List<int> CarsInQue2Dynamics { get; set; }

        public Statistics()
        {
            SimulationTime = 0;
            TotalCars = 0;
            TotalCivilCars = 0;
            TotalGovCars = 0;
            MinTravelTime = 0;
            MaxTravelTime = 0;
            MaxWaitingTime = 0;
            TotalCarsInStream1 = 0;
            TotalCarsInStream2 = 0;
            AvgWaitingTimeInStream1 = 0;
            AvgWaitingTimeInStream2 = 0;
            AvgServeTimeInStream1 = 0;
            AvgServeTimeInStream2 = 0;
            CarsInQue1Dynamics = new();
            CarsInQue2Dynamics = new();
        }


        /// <summary>
        /// Записывает 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="time"></param>
        public void AddCarsInQueToStat(ServeStream stream)
        {
            if (stream.InStream1.InputQueue != null)
            {
                CarsInQue1Dynamics.Add(stream.InStream1.InputQueue.Count);
            }

            if (stream.InStream2.InputQueue != null)
            {
                CarsInQue2Dynamics.Add(stream.InStream2.InputQueue.Count);
            }
        }

        /// <summary>
        /// Обработка статистики из списка обслуженных машин. Данные в коллекциях 
        /// на выходе представлены почасовыми показателями.
        /// </summary>
        /// <param name="servedCars">Коллекция обслуженных машин</param>
        /// <param name="simulationTime">Время симуляции</param>
        public void Process(List<Car> servedCars, int simulationTime)
        {

            SimulationTime = simulationTime;
            TotalCars = servedCars.Count;
            TotalCivilCars = servedCars.Where(x => x is CivilCar).Count();
            TotalGovCars = servedCars.Where(x => x is GovCar).Count();
            MaxTravelTime = servedCars.Max(x => x.TravelTime);
            MinTravelTime = servedCars.Min(x => x.TravelTime);
            TotalCarsInStream1 = servedCars.Where(x => x.Origin == "Input Stream 1").Count();
            TotalCarsInStream2 = servedCars.Where(x => x.Origin == "Input Stream 2").Count();
            AvgWaitingTimeInStream1 = servedCars.Where(x => x.Origin == "Input Stream 1").Sum(x => x.WaitingTime) / TotalCarsInStream1;
            AvgWaitingTimeInStream2 = servedCars.Where(x => x.Origin == "Input Stream 2").Sum(x => x.WaitingTime) / TotalCarsInStream2;
            AvgServeTimeInStream1 = servedCars.Where(x => x.Origin == "Input Stream 1").Sum(x => x.TravelTime) / TotalCarsInStream1;
            AvgServeTimeInStream2 = servedCars.Where(x => x.Origin == "Input Stream 2").Sum(x => x.TravelTime) / TotalCarsInStream2;

            //CarsInQue2Dynamics = ConvertMinutsToHours(CarsInQue2Dynamics);
            //CarsInQue1Dynamics = ConvertMinutsToHours(CarsInQue1Dynamics);
        }

        /// <summary>
        /// Расчет почасовых показателей для входных потоков. Для построение графиков.
        /// </summary>
        /// <param name="cars">Коллекция обслуженных машин</param>
        /// <returns>Почасовые показатели.</returns>
        public static List<int> ConvertMinutsToHours(List<int> list)
        {
            List<int> result = new();
            var counter = 0;
            var sum = 0;
            foreach(var item in list)
            {
                counter++;
                sum += item;
                if (counter == 60)
                {
                    result.Add(sum/60);
                    counter = 0;
                    sum = 0;
                }
            }
            return result;
        }

        /// <summary>
        /// Очистка всех свойств экземпляра класса Statistics.
        /// </summary>
        public void ClearAllStats()
        {
            foreach (var propertyInfo in this.GetType().GetProperties())
                ClearProperty(propertyInfo);
        }

        /// <summary>
        /// Очистка значения свойства. Поддерживаются типы Int32 и List(Int32).
        /// </summary>
        /// <param name="propertyInfo">Свойство</param>
        /// <exception cref="Exception">Тип свойства не поддерживается</exception>
        public void ClearProperty(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType == typeof(List<int>))
                propertyInfo.SetValue(this, new List<int>());
            else if (propertyInfo.PropertyType == typeof(int))
                propertyInfo.SetValue(this, 0);
            else
                throw new Exception("Unknown type in ClearProperty method.");
        }
    }
}
