using System.Collections.Generic;
using System.Windows.Controls;

namespace TrafficModeling.View
{
    public partial class Page1 : Page
    {
        public Page1(List<int> points1, List<int> points2)
        {
            InitializeComponent();

            DataContext = new Page1VM(points1, points2);
        }
    }
}
