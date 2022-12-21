namespace TrafficModeling.Model
{
    /// <summary>
    /// Интерфейс Издатель паттерна Наблюдатель
    /// </summary>
    internal interface ITime
    {
        /// <summary>
        /// Текущее время
        /// </summary>
        public int CurrentTime { get; }

        /// <summary>
        /// Присоединяет наблюдателя к издателю.
        /// </summary>
        /// <param name="observer">Наблюдатель</param>
        void Attach(ITimeObserver observer);

        /// <summary>
        /// Отсоединяет наблюдателя от издателя.
        /// </summary>
        /// <param name="observer">Наблюдатель</param>
        void Detach(ITimeObserver observer);

        /// <summary>
        /// Уведомляет всех наблюдателей о событии.
        /// </summary>
        void Notify();
    }
}
