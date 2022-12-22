namespace TrafficModeling.Model
{
    /// <summary>
    /// Светофор, в Q-схеме выполняет роль клапана для входящего потока. Реализует интерфейс Наблюдатель.
    /// </summary>
    internal class TrafficLight : ITimeObserver
    {
        /// <summary>
        /// Длительность зеленого света в тиках
        /// </summary>
        public static int LightTime { get; set; }

        /// <summary>
        /// Задержка перед переключением с красного на зеленый. Равна времени зеленого света + 2 задержки (когда оба светофора красные).
        /// </summary>
        public static int Delay { get; set; }

        /// <summary>
        /// Счетчик времени
        /// </summary>
        public int TimeCounter { get; set; }

        /// <summary>
        /// Фаза светофора
        /// </summary>
        public bool IsGreen { get; set; }

        public TrafficLight(int lightTime, int delay, bool IsGreen)
        {
            LightTime = lightTime;
            Delay = 2 * delay + lightTime; // задержка = время зеленой фазы + время общей красной фазы
            this.IsGreen = IsGreen;
            if (!IsGreen) // фаза "посередине"
                TimeCounter = delay;
            else
                TimeCounter = 0;
        }

        /// <summary>
        /// Реализация Update паттерна Наблюдатель. Переключение светофора в необходимый момент времени. 
        /// </summary>
        /// <param name="time">Текущее время симуляции</param>
        public void Update(ITime time)
        {
            IncrementTimer();
        }

        /// <summary>
        /// Инкремент внутреннего таймера светофора для отслеживания момента времени для переключения сигнала.
        /// </summary>
        public void IncrementTimer()
        {
            TimeCounter++;

            if (IsGreen)
            {
                if (TimeCounter == LightTime)
                    SwitchSignal();
            }
            else
            {
                if (TimeCounter == Delay)
                    SwitchSignal();
            }
        }

        /// <summary>
        /// Переключение зеленого сигнала. Обнуление счетчика и реверсия цвета.
        /// </summary>
        public void SwitchSignal()
        {
            IsGreen = !IsGreen;
            TimeCounter = 0;
        }
    }
}
