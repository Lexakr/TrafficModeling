using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using TrafficModeling.Model;
using TrafficModeling.Properties;
using TrafficModeling.View;

namespace TrafficModeling.ViewModel
{
    internal partial class MainWindowVM : ObservableValidator
    {
        [ObservableProperty]
        private ChartsVM chartVM;

        [ObservableProperty]
        public Simulation simulat;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(40, 90)]
        private int civilCarSpeed;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(0.1, 10)]
        private double civilCarSpeedDeviance;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(60, 100)]
        private int govermentCarSpeed;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(0.1, 10)]
        private double govermentCarSpeedDeviance;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(2, 60)]
        private double inputStream1ExpValue;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(0.1, 10)]
        private double inputStream1Dispersion;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(2, 60)]
        private double inputStream2ExpValue;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(0.1, 10)]
        private double inputStream2Dispersion;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(5, 600)]
        private int trafficLightTime;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(5, 600)]
        private int trafficLightDelay;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(100, 5000)]
        private int roadLength;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(1, 72)]
        private int simulationTime;

        public MainWindowVM()
        {
            Simulat = new();
            chartVM = new();

            // Инициализация 
            CivilCarSpeed = Settings.Default.CivilCarSpeed;
            CivilCarSpeedDeviance = Settings.Default.CivilCarSpeedDeviance;
            GovermentCarSpeed = Settings.Default.GovermentCarSpeed;
            GovermentCarSpeedDeviance = Settings.Default.GovermentCarSpeedDeviance;

            InputStream1ExpValue = Settings.Default.InputStream1ExpValue;
            InputStream1Dispersion = Settings.Default.InputStream1Dispersion;
            InputStream2ExpValue = Settings.Default.InputStream2ExpValue;
            InputStream2Dispersion = Settings.Default.InputStream2Dispersion;

            TrafficLightTime = Settings.Default.TrafficLightTime;
            TrafficLightDelay = Settings.Default.TrafficLightDelay;
            SimulationTime = Settings.Default.SimulationTime;
            RoadLength = Settings.Default.RoadLength;
        }

        // Команда старт
        [RelayCommand]
        private void Start()
        {
            // Валидация всех текстБоксов
            ValidateAllProperties();

            if (HasErrors)
            {
                MessageBox.Show("Enter correct simulation parameters!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveSettings();

            Simulat = new(simulationTime * 36000, inputStream1ExpValue, inputStream1Dispersion, inputStream2ExpValue, inputStream2Dispersion, trafficLightTime,
                trafficLightDelay, roadLength, civilCarSpeed, civilCarSpeedDeviance, govermentCarSpeed, govermentCarSpeedDeviance);
            try
            {
                Simulat.Run();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Simulation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            chartVM.CreateChart(Simulat.SimStats.CarsInQue1Dynamics, Simulat.SimStats.CarsInQue2Dynamics);

        }

        /// <summary>
        /// Сохранение настроек из текстБоксов представления.
        /// </summary>
        private void SaveSettings()
        {
            Settings.Default.CivilCarSpeed = CivilCarSpeed;
            Settings.Default.CivilCarSpeedDeviance = CivilCarSpeedDeviance;
            Settings.Default.GovermentCarSpeed = GovermentCarSpeed;
            Settings.Default.GovermentCarSpeedDeviance = GovermentCarSpeedDeviance;
            Settings.Default.InputStream1ExpValue = InputStream1ExpValue;
            Settings.Default.InputStream1Dispersion = InputStream1Dispersion;
            Settings.Default.InputStream2ExpValue = InputStream2ExpValue;
            Settings.Default.InputStream2Dispersion = InputStream2Dispersion;
            Settings.Default.TrafficLightTime = TrafficLightTime;
            Settings.Default.TrafficLightDelay = TrafficLightDelay;
            Settings.Default.RoadLength = RoadLength;
            Settings.Default.SimulationTime = SimulationTime;
            Settings.Default.Save();
        }
    }
}
