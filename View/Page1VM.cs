using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace TrafficModeling.View
{
    [ObservableObject]
    public partial class Page1VM
    {
        public Page1VM(List<int> points)
        {
            var values = new int[points.Count / 60];
            //values[0] = 0;
            var t = 0;
            var tmp = 0;
            int sum = 0;
            for(int i = 0; i < points.Count; i++)
            {
                tmp++;
                sum += points[i];
                if (tmp == 60)
                {
                    values[t] = sum;
                    sum = 0;
                    t++;
                    tmp = 0;
                }
            }
            t = 0;


            /*            var values = new int[points.Count];
                        var t = 0;
                        for (var i = 0; i < points.Count; i++)
                        {
                            t = points[i];
                            values[i] = t;
                        }*/





            SeriesCollection = new ISeries[]
            { new LineSeries<int>
                {
                    GeometrySize = 0,
                    Values = values,
                }
            };
        }
        
        public ISeries[] SeriesCollection { get; set; }
    }
}
