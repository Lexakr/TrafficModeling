using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows;
using TrafficModeling.Model;
using TrafficModeling.Properties;
using TrafficModeling.View;

namespace TrafficModeling.ViewModel
{
    /// <summary>
    /// Главная модель представления
    /// </summary>
    internal partial class MainWindowVM : ObservableValidator
    {
        [ObservableProperty]
        private ChartsVM chartVM;

        [ObservableProperty]
        private Simulation simulat;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(40, 120)]
        private string civilCarSpeed;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(0.1, 9.0)]
        [RegularExpression(@"^[0-9,]+$", ErrorMessage = "Десятичные дроби должны быть в формате 0,1")]
        private string civilCarSpeedDeviance;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(40, 120)]
        [RegularExpression(@"^[0-9,]+$", ErrorMessage = "Десятичные дроби должны быть в формате 0,1")]
        private string govermentCarSpeed;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(0.1, 9.0)]
        [RegularExpression(@"^[0-9,]+$", ErrorMessage = "Десятичные дроби должны быть в формате 0,1")]
        private string govermentCarSpeedDeviance;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(2.0, 60.0)]
        [RegularExpression(@"^[0-9,]+$", ErrorMessage = "Десятичные дроби должны быть в формате 0,1")]
        private string inputStream1ExpValue;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(0.1, 9.0)]
        [RegularExpression(@"^[0-9,]+$", ErrorMessage = "Десятичные дроби должны быть в формате 0,1")]
        private string inputStream1Dispersion;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(2.0, 60.0)]
        [RegularExpression(@"^[0-9,]+$", ErrorMessage = "Десятичные дроби должны быть в формате 0,1")]
        private string inputStream2ExpValue;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(0.1, 9.0)]
        [RegularExpression(@"^[0-9,]+$", ErrorMessage = "Десятичные дроби должны быть в формате 0,1")]
        private string inputStream2Dispersion;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(5, 1800)]
        private string trafficLightTime;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(5, 1800)]
        private string trafficLightDelay;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(100, 5000)]
        private string roadLength;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required]
        [Range(1, 24)]
        private string simulationTime;

        [ObservableProperty]
        private bool isWindowEnabled = true;

        public MainWindowVM()
        {
            Simulat = new();
            chartVM = new();

            // Инициализация сохраненных параметров
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

        /// <summary>
        /// Команда старт - начать симуляцию.
        /// </summary>
        [RelayCommand]
        private async void Start()
        {
            // Валидация всех текстБоксов
            ValidateAllProperties();

            if (HasErrors)
            {
                MessageBox.Show("Введите корректные параметры симуляции!", "Ошибка Ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            IsWindowEnabled = false;

            // Сохраняем настройки симуляции
            SaveSettings();

            Simulat = new(Int32.Parse(simulationTime) * 36000, Double.Parse(inputStream1ExpValue), Double.Parse(inputStream1Dispersion),
                Double.Parse(inputStream2ExpValue), Double.Parse(inputStream2Dispersion), Int32.Parse(trafficLightTime),
                Int32.Parse(trafficLightDelay), Int32.Parse(roadLength), Double.Parse(civilCarSpeed), Double.Parse(civilCarSpeedDeviance),
                Double.Parse(govermentCarSpeed), Double.Parse(govermentCarSpeedDeviance));

            await Task.Run(() =>
            {
                try
                {
                    Simulat.Run();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка Симуляции", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            });

            // Строит графики по данным статистики симуляции
            chartVM.CreateChart(Simulat.SimStats.CarsInQue1Dynamics, Simulat.SimStats.CarsInQue2Dynamics);

            IsWindowEnabled = true;
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
