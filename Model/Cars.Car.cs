namespace TrafficModeling.Model
{
    /// <summary>
    /// Машина, передвигающаяся по дороге. В Q-схеме представляет собой заявку.
    /// </summary>
    internal class Car
    {
        /// <summary>
        /// Уникальное имя
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Поток, в котором был создан экземпляр
        /// </summary>
        public string Origin { get; }

        /// <summary>
        /// Скорость в км/ч
        /// </summary>
        public int Speed { get; }

        /// <summary>
        /// Количество тиков до покидания обслуживающего потока 
        /// </summary>
        public int TravelTimeLeft { get; set; }

        /// <summary>
        /// Изначальное количество тиков на прохождение обслуживаюего потока
        /// </summary>
        public int TravelTime { set; get; }

        /// <summary>
        /// Время, затраченное на ожидание перед светофором
        /// </summary>
        public int WaitingTime { get; set; }

        /// <summary>
        /// Время симуляции, когда экземпляр прибыл в очередь входного потока. Используется для вычисления времени простоя в очереди
        /// </summary>
        public int ArrivalTime { get; set; }

        public Car(string name, int speed, string origin, int arrivalTime)
        {
            Name = name;
            Speed = speed;
            ArrivalTime = arrivalTime;
            Origin = origin;

            TravelTimeLeft = 0;
            TravelTime = 0;
            WaitingTime = 0;
        }
    }
}
