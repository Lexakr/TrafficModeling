using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TrafficModeling.Model;
using TrafficModeling.Presenters;
using TrafficModeling.View;

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
            model = new();
            presenter = new(model);

            CivCarSpeed.Text = Properties.Settings.Default.civCarV.ToString();
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            if (!SettingsValidation())
                return;

            this.IsEnabled = false;

            model.simStats.Clear();
            presenter.Run();

            this.IsEnabled = true;



            tabItems = new()
            {
                new TabItem()
                {
                    Header = "Graph_1",

                    Content = new Frame()
                    {
                        Content = new Page1(model.simStats.CarsInQueDynamics)
                    }
                }
            };
            /*
                        Frame tabFrame = new Frame();
                        Page1 page1 = new Page1(model.simStats.CarsInQueDynamics);
                        tabFrame.Content = page1;
                        tabitem.Content = tabFrame;
                        Tab_Model.Items.Add(tabitem);*/

            for (int i = 1; i < Tab_Model.Items.Count; i++)
            {
                Tab_Model.Items.RemoveAt(i);
            }

            foreach (TabItem item in tabItems)
            {
                Tab_Model.Items.Add(item);
            }
        }

        private bool SettingsValidation()
        {
            if (double.TryParse(CivCarSpeed.Text, out var civCarSpeed))
                Properties.Settings.Default.civCarV = civCarSpeed;
            else
            {
                MessageBox.Show("validation er"); 
                return false;
            }
            if (double.TryParse(CivCarStdDev.Text, out var result))
                Properties.Settings.Default.civCarV = result;
            else
                MessageBox.Show("validation er");

            Properties.Settings.Default.Save();
            return true;
        }

    }
}
