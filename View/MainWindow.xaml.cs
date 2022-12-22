using System.Windows;
using TrafficModeling.ViewModel;

namespace TrafficModeling
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowVM();
        }
    }
}
