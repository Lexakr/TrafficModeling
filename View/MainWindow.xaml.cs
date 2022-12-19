using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TrafficModeling.Model;
using TrafficModeling.Presenters;
using TrafficModeling.View;
using System.Windows.Input;
using System.Text.RegularExpressions;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Threading;

namespace TrafficModeling
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Simulation model;
        private Presenter presenter;
        private List<TabItem> tabItems;

        public MainWindow()
        {
            InitializeComponent();

            // Инициализация 
            CivCarSpeed.Text = Properties.Settings.Default.civCarV.ToString();
            CivCarStdDev.Text = Properties.Settings.Default.civCarDev.ToString();
            GovCarSpeed.Text = Properties.Settings.Default.govCarV.ToString();
            GovCarStdDev.Text = Properties.Settings.Default.govCarDev.ToString();

            InputStream1ExpValue.Text = Properties.Settings.Default.road1time.ToString();
            InputStream1Dispersion.Text = Properties.Settings.Default.road1disp.ToString();
            InputStream2ExpValue.Text = Properties.Settings.Default.road2time.ToString();
            InputStream2Dispersion.Text = Properties.Settings.Default.road2disp.ToString();

            TrafficLightTime.Text = Properties.Settings.Default.greenlight.ToString();
            TrafficLightDelay.Text = Properties.Settings.Default.redlight.ToString();
            SimulationTime.Text = Properties.Settings.Default.timer.ToString();
            Length.Text = Properties.Settings.Default.length.ToString();
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;

            if (!SettingsValidation())
                return;

            model = new(Properties.Settings.Default.timer * 36000, Properties.Settings.Default.road1time, Properties.Settings.Default.road1disp,
                Properties.Settings.Default.road2time, Properties.Settings.Default.road2disp, Properties.Settings.Default.greenlight,
                Properties.Settings.Default.redlight, Properties.Settings.Default.length, Properties.Settings.Default.civCarV,
                Properties.Settings.Default.civCarDev, Properties.Settings.Default.govCarV, Properties.Settings.Default.govCarDev);
            presenter = new(model);

            DataContext = model;

            model.SimStats.ClearAllStats();

            presenter.RunSimulation();

            this.IsEnabled = true;

            tabItems = new()
            {
                new TabItem()
                {
                    Header = "Graph_1",

                    Content = new Frame()
                    {
                        Content = new Page1(model.SimStats.CarsInQue1Dynamics, model.SimStats.CarsInQue2Dynamics)
                    }
                }
            };

            for (int i = 2; i < Tab_Model.Items.Count; i++)
            {
                Tab_Model.Items.RemoveAt(i);
            }

            foreach (TabItem item in tabItems)
            {
                Tab_Model.Items.Add(item);
            }
        }

        /// <summary>
        /// Валидация введенных пользователем данных для симуляции.
        /// </summary>
        /// <returns>true or false</returns>
        private bool SettingsValidation()
        {
            // Civil Cars generator parameters
            if (double.TryParse(CivCarSpeed.Text, out var civCarSpeed) && civCarSpeed >= 40 && civCarSpeed <= 90)
                Properties.Settings.Default.civCarV = civCarSpeed;
            else
            {
                MessageBox.Show("Enter civil cars speed from 40 to 90", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error); 
                return false;
            }
            if (double.TryParse(CivCarStdDev.Text, out var civCarStdDev) && civCarStdDev >= 0.1 && civCarStdDev <= 4)
                Properties.Settings.Default.civCarDev = civCarStdDev;
            else
            {
                MessageBox.Show("Enter civil cars variance from 0.1 to 4", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Goverment Cars generator parameters
            if (double.TryParse(GovCarSpeed.Text, out var govCarSpeed) && govCarSpeed >= 60 && govCarSpeed <= 110)
                Properties.Settings.Default.govCarV = govCarSpeed;
            else
            {
                MessageBox.Show("Enter goverment cars speed from 60 to 110", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (double.TryParse(GovCarStdDev.Text, out var govCarStdDevd) && govCarStdDevd >= 0.1 && govCarStdDevd <= 4)
                Properties.Settings.Default.govCarDev = govCarStdDevd;
            else
            {
                MessageBox.Show("Enter goverment cars variance from 0.1 to 4", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // First Input Stream car generator parameters
            if (double.TryParse(InputStream1ExpValue.Text, out var inputStream1ExpValue) && inputStream1ExpValue >= 2 && inputStream1ExpValue <= 120)
                Properties.Settings.Default.road1time = inputStream1ExpValue;
            else
            {
                MessageBox.Show("Enter 1st road timer from 2 to 120", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (double.TryParse(InputStream1Dispersion.Text, out var inputStream1StdDev) && inputStream1StdDev >= 0.1 && inputStream1StdDev <= 4)
                Properties.Settings.Default.road1disp = inputStream1StdDev;
            else
            {
                MessageBox.Show("Enter 1st road timer dispersion from 0.1 to 4", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Second Input Stream car generator parameters
            if (double.TryParse(InputStream2ExpValue.Text, out var inputStream2ExpValue) && inputStream2ExpValue >= 2 && inputStream2ExpValue <= 120)
                Properties.Settings.Default.road2time = inputStream2ExpValue;
            else
            {
                MessageBox.Show("Enter 2nd road timer from 2 to 120", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (double.TryParse(InputStream2Dispersion.Text, out var inputStream2StdDev) && inputStream2StdDev >= 0.1 && inputStream2StdDev <= 4)
                Properties.Settings.Default.road2disp = inputStream2StdDev;
            else
            {
                MessageBox.Show("Enter 2nd road timer dispersion from 0.1 to 4", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Traffic Lights parameters
            if (int.TryParse(TrafficLightTime.Text, out var trafficLightTime) && trafficLightTime >= 10 && trafficLightTime <= 300)
                Properties.Settings.Default.greenlight = trafficLightTime;
            else
            {
                MessageBox.Show("Enter traffic light green timer from 10 to 300", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (int.TryParse(TrafficLightDelay.Text, out var trafficLightDelay) && trafficLightDelay >= 30 && trafficLightDelay <= 600)
                Properties.Settings.Default.redlight = trafficLightDelay;
            else
            {
                MessageBox.Show("Enter traffic light delay from 30 to 600", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Simulation parameters
            if (int.TryParse(SimulationTime.Text, out var simulationTime) && simulationTime >= 1 && simulationTime <= 168)
                Properties.Settings.Default.timer = simulationTime;
            else
            {
                MessageBox.Show("Enter simulation timer from 1 to 168 hours", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (int.TryParse(Length.Text, out var length) && length >= 500 && length <= 5000)
                Properties.Settings.Default.length = length;
            else
            {
                MessageBox.Show("Enter road length from 500 to 5000", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            Properties.Settings.Default.Save();
            return true;
        }
    }
}
