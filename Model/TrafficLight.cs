namespace TrafficModeling.Model
{
    /// <summary>
    /// Светофор в Q-схеме выполняет роль клапана для двух потоков
    /// </summary>
    internal class TrafficLight
    {
        // длительность зеленого света
        public static int LightTime { get; set; }
        // задержка перед переключением с красного на зеленый, когда противоположный светофор загорелся красным
        public static int Delay { get; set; }
        public int Counter { get; set; }
        public bool IsGreen { get; set; }

        public TrafficLight(bool IsGreen)
        {
            this.IsGreen = IsGreen;
        }
        public TrafficLight(int lightTime, int delay, bool IsGreen)
        {
            LightTime = lightTime;
            Delay = delay + lightTime; // задержка = время зеленой фазы + время общей красной фазы
            this.IsGreen = IsGreen;
            if (!IsGreen) // фаза "посередине"
                Counter = (delay - lightTime) / 2;
            else 
                Counter = 0;
        }

        /// <summary>
        /// Реакция на dt светофором. Низкий таймер для зеленой фазы, высокий (+ задержка) для красной.
        /// </summary>
        /// <param name="time"></param>
        public void Update()
        {
            if (IsGreen)
            {
                Counter++;
                if (Counter == LightTime) SwitchSignal();
            }
            else
            {
                Counter++;
                if (Counter == Delay) SwitchSignal();
            }
        }

        /// <summary>
        /// Переключение зеленого сигнала. Обнуление счетчика и реверсия цвета.
        /// </summary>
        public void SwitchSignal()
        {
            IsGreen = !IsGreen;
            Counter = 0;
        }
    }
}
