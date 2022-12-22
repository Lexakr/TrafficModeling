using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace TrafficModeling.Model
{
    /// <summary>
    /// Статистика по завершенной симуляции.
    /// </summary>
    [INotifyPropertyChanged]
    internal partial class Statistics
    {
        /// <summary>
        /// Общее время симуляции
        /// </summary>
        [ObservableProperty]
        private int totalSimulationTime;

        /// <summary>
        /// Всего машин обслужено
        /// </summary>
        [ObservableProperty]
        private int totalCars;

        /// <summary>
        /// Гражданских машин обслужено
        /// </summary>
        [ObservableProperty]
        private int totalCivilCars;

        /// <summary>
        /// Машин спецслужб обслужено
        /// </summary>
        [ObservableProperty]
        private int totalGovCars;

        /// <summary>
        /// Машина, проехавшая участок быстрее всех
        /// </summary>
        [ObservableProperty]
        private int maxTravelTime;

        /// <summary>
        /// Машина, проехавшая участок медленнее всех
        /// </summary>
        [ObservableProperty]
        private int minTravelTime;

        /// <summary>
        /// Наибольшее время ожидания в очереди (для обслуженных машин)
        /// </summary>
        [ObservableProperty]
        private int maxWaitingTime;

        /// <summary>
        /// Всего обслужено из 1 очереди
        /// </summary>
        [ObservableProperty]
        private int totalCarsInStream1;

        /// <summary>
        /// Всего обслужено из 2 очереди
        /// </summary>
        [ObservableProperty]
        private int totalCarsInStream2;

        /// <summary>
        /// Среднее время ожидания в 1 очереди
        /// </summary>
        [ObservableProperty]
        private int avgWaitingTimeInStream1;

        /// <summary>
        /// Среднее время ожидания в0 2 очереди
        /// </summary>
        [ObservableProperty]
        private int avgWaitingTimeInStream2;

        /// <summary>
        /// Среднее время проезда машин из 1 очереди
        /// </summary>
        [ObservableProperty]
        private int avgServeTimeInStream1;

        /// <summary>
        /// Среднее время проезда машин из 2 очереди
        /// </summary>
        [ObservableProperty]
        private int avgServeTimeInStream2;

        /// <summary>
        /// Динамика машин в 1 очереди
        /// </summary>
        [ObservableProperty]
        private List<int> carsInQue1Dynamics;

        /// <summary>
        /// Динамика машин во 2 очереди
        /// </summary>
        [ObservableProperty]
        private List<int> carsInQue2Dynamics;

        /// <summary>
        /// Количество необслуженных машин в очереди к концу симуляции.
        /// </summary>
        [ObservableProperty]
        private int carsInQueue;

        /// <summary>
        /// Конструктор по умолчанию задает все параметры нулями.
        /// </summary>
        public Statistics()
        {
            TotalSimulationTime = 0;
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
        /// Записывает количество машин в очередях в статистику.
        /// </summary>
        /// <param name="stream">Поток обслуживания</param>
        public void AddCarsInQueToStat(ServeStream stream)
        {
            if (stream.InStream1.InputQue != null)
            {
                CarsInQue1Dynamics.Add(stream.InStream1.InputQue.Count);
            }

            if (stream.InStream2.InputQue != null)
            {
                CarsInQue2Dynamics.Add(stream.InStream2.InputQue.Count);
            }
        }

        /// <summary>
        /// Обработка статистики из списка обслуженных машин.
        /// </summary>
        /// <param name="servedCars">Коллекция обслуженных машин</param>
        /// <param name="carsInQue">Коллекция необслуженных машин</param>
        /// <param name="simulationTime">Время симуляции</param>
        public void Process(List<Car> servedCars, int carsInQue, int simulationTime)
        {
            TotalSimulationTime = simulationTime;
            TotalCars = servedCars.Count;
            TotalCivilCars = servedCars.Where(x => x is CivilCar).Count();
            TotalGovCars = servedCars.Where(x => x is GovCar).Count();
            MaxTravelTime = servedCars.Max(x => x.TravelTime);
            MinTravelTime = servedCars.Min(x => x.TravelTime);
            MaxWaitingTime = servedCars.Max(x => x.WaitingTime);
            TotalCarsInStream1 = servedCars.Where(x => x.Origin == "Input Stream 1").Count();
            TotalCarsInStream2 = servedCars.Where(x => x.Origin == "Input Stream 2").Count();
            AvgWaitingTimeInStream1 = servedCars.Where(x => x.Origin == "Input Stream 1").Sum(x => x.WaitingTime) / TotalCarsInStream1;
            AvgWaitingTimeInStream2 = servedCars.Where(x => x.Origin == "Input Stream 2").Sum(x => x.WaitingTime) / TotalCarsInStream2;
            AvgServeTimeInStream1 = servedCars.Where(x => x.Origin == "Input Stream 1").Sum(x => x.TravelTime) / TotalCarsInStream1;
            AvgServeTimeInStream2 = servedCars.Where(x => x.Origin == "Input Stream 2").Sum(x => x.TravelTime) / TotalCarsInStream2;

            CarsInQueue = carsInQue;
        }

        /// <summary>
        /// Очистка всех свойств экземпляра класса Statistics.
        /// </summary>
        public void ClearAllProperties()
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
