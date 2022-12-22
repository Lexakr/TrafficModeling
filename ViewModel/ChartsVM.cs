using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveCharts.Helpers;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;

namespace TrafficModeling.View
{
    [ObservableObject]
    public partial class ChartsVM
    {
        [ObservableProperty]
        public ISeries[] seriesCollection;

        [ObservableProperty]
        public Axis[] xAxes;

        [ObservableProperty]
        public Axis[] yAxes;

        public ChartsVM()
        {

        }

        public void CreateChart(List<int> points1, List<int> points2)
        {
            SeriesCollection = new ISeries[]
            {
                new LineSeries<int>
                {
                    Name = "Queue 1 cars",
                    GeometrySize = 0,
                    Fill = null,
                    Values = points1.AsChartValues(),
                    LineSmoothness = 0.2,
                },
                new LineSeries<int>
                {
                    Name = "Queue 2 cars",
                    GeometrySize = 0,
                    Fill = null,
                    Values = points2.AsChartValues(),
                    LineSmoothness = 0.2,
                }
            };

            XAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Time in minuts",
                }
            };

            YAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Cars",
                }
            };
        }

        [RelayCommand]
        public void ToggleSeries0()
        {
            SeriesCollection[0].IsVisible = !SeriesCollection[0].IsVisible;
        }

        [RelayCommand]
        public void ToggleSeries1()
        {
            SeriesCollection[1].IsVisible = !SeriesCollection[1].IsVisible;
        }
    }
}
