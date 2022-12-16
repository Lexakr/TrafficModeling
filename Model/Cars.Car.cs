﻿using System;

namespace TrafficModeling.Model
{
    internal abstract class Car
    {
        public string Name { get; }
        public string Origin { get; }
        public int Speed { get; }
        // Сколько тиков на прохождение участка (1т = 1 децисекунда)
        public int TravelTimeLeft { get; set; }
        public int TravelTime { set; get; }
        public int WaitingTime { get; set; } // ожидание перед светофором
        public bool Priority { get; set; }

        public Car(string Name, int Speed, string Origin)
        {
            this.Name = Name;
            this.Speed = Speed;
            TravelTimeLeft = 0;
            TravelTime = 0;
            WaitingTime = 0;
            Priority = false;
            this.Origin = Origin;
        }
    }
}