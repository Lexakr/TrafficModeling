using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TrafficModeling.Model;

namespace TrafficModeling.Presenters
{
    /// <summary>
    /// Presetner class from MVP pattern.
    /// </summary>
    internal class Presenter
    {
        private Simulation _simulation; // Model
        private MainWindow _mainWindow; // View 

        public Presenter(Simulation simulation, MainWindow mainWindow)
        {
            _simulation = simulation;
            _mainWindow = mainWindow;
        }

        public void RunSimulation() // Запуск процесса моделирования
        {
            _simulation.Run(); //Обновление модели
        }

        public void ValidateUserInput()
        {

        }
    }
}
