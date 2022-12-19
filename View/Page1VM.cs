using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using LiveCharts.Helpers;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.Measure;

namespace TrafficModeling.View
{
    [ObservableObject]
    public partial class Page1VM
    {
        private static readonly SKColor s_red = new(229, 57, 53);
        public Page1VM(List<int> points1, List<int> points2)
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
                    GeometryStroke = new SolidColorPaint(s_red, 2),
                    Fill = null,
                    Values = points2.AsChartValues(),
                    LineSmoothness = 0.2,
                }
            };

            YAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Cars in queues dynamics",
                }
            };
        }


        public ISeries[] SeriesCollection { get; set; }

        public Axis[] XAxes { get; set; } =
        {
            new Axis
            {
                Name = "Time in minuts",
                NameTextSize = 20
            }
        };
        public Axis[] YAxes { get; set; }

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
