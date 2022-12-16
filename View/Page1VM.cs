using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TrafficModeling.View
{
    [ObservableObject]
    public partial class Page1VM
    {
        public Page1VM(List<int> points)
        {
            var values = new int[points.Count];
            var t = 0;
            for (var i = 0; i < points.Count; i++)
            {
                t = points[i];
                values[i] = t;
            }



            SeriesCollection = new ISeries[] { new LineSeries<int> { Values = values } };
        }

        public Page1VM()
        {

            var values = new int[100];
            var r = new Random();
            var t = 0;

            for (var i = 0; i < 100; i++)
            {
                t += r.Next(-90, 100);
                values[i] = t;
            }

            SeriesCollection = new ISeries[] { new LineSeries<int> { Values = values } };
        }

        public ISeries[] SeriesCollection { get; set; }
        public void foo()
        {
            
        }
    }
}
