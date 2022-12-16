using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TrafficModeling.Model;

namespace TrafficModeling.Presenters
{
    internal class Presenter
    {
        private Simulation model; //Связь с моделью

        public Presenter(Simulation model)
        {
            this.model = model;
        }

        public string Run() // Запуск процесса моделирования
        {
            //SaveData();
            model.RunSimulation(); //Обновление модели
            return model.simStats.TotalCars.Last().ToString(); //Передача обновлённой модели
        }

        public void SaveData()
        {

        }
    }
}
