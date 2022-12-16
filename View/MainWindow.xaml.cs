using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TrafficModeling.Model;
using TrafficModeling.Presenters;
using LiveCharts.Wpf;
using LiveCharts;
using System.Reflection.Emit;
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

        public SeriesCollection SeriesCollection { get; set; } // add clear collection
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            model = new();
            presenter = new(model);

            // CHARTS
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Series 3",
                    Values = new ChartValues<double> { 4,2,7,2,7 },
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }
            };

            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => value.ToString("C");

            //modifying the series collection will animate and update the chart
            SeriesCollection.Add(new LineSeries
            {
                Title = "Series 4",
                Values = new ChartValues<double> { 5, 3, 2, 4 },
                LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                PointGeometrySize = 50,
                PointForeground = Brushes.Gray
            });

            //modifying any series values will also animate and update the chart
            SeriesCollection[3].Values.Add(5d);

            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            MessageBox.Show(presenter.Run());
            this.IsEnabled = true;

            TabItem tabitem = new TabItem();
            tabitem.Header = "Tab 3";
            Frame tabFrame = new Frame();
            Page1 page1 = new Page1(model.simStats.TotalCars);
            tabFrame.Content = page1;
            tabitem.Content = tabFrame;
            Tab_Model.Items.Add(tabitem);


        }
        

    }
}
