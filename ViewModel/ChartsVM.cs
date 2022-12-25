using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveCharts.Helpers;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.Generic;

namespace TrafficModeling.View
{
    /// <summary>
    /// Модель представления для графиков
    /// </summary>
    [ObservableObject]
    public partial class ChartsVM
    {
        /// <summary>
        /// Коллекция графиков (график - массив значений(точек))
        /// </summary>
        [ObservableProperty]
        public ISeries[] seriesCollection;

        /// <summary>
        /// Ось абсцисс
        /// </summary>
        [ObservableProperty]
        public Axis[] xAxes;

        /// <summary>
        /// Ось ординат
        /// </summary>
        [ObservableProperty]
        public Axis[] yAxes;

        public ChartsVM()
        {

        }

        /// <summary>
        /// Инициализирует коллекцию графиков по заданным массивам точек
        /// </summary>
        /// <param name="points1">Первый массив точек</param>
        /// <param name="points2">Второй массив точек</param>
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
                    Stroke = new SolidColorPaint(SKColors.YellowGreen, 1.5f),
                    GeometryStroke = new SolidColorPaint(SKColors.YellowGreen, 1.5f)
                },
                new LineSeries<int>
                {
                    Name = "Queue 2 cars",
                    GeometrySize = 0,
                    Fill = null,
                    Values = points2.AsChartValues(),
                    LineSmoothness = 0.2,
                    Stroke = new SolidColorPaint(SKColors.Blue, 1.5f),
                    GeometryStroke = new SolidColorPaint(SKColors.Blue, 1.5f)
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
            if (SeriesCollection != null && SeriesCollection[0] != null)
                SeriesCollection[0].IsVisible = !SeriesCollection[0].IsVisible;
        }

        [RelayCommand]
        public void ToggleSeries1()
        {
            if (SeriesCollection != null && SeriesCollection[1] != null)
                SeriesCollection[1].IsVisible = !SeriesCollection[1].IsVisible;
        }
    }
}
