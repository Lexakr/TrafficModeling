namespace TrafficModeling.Model
{
    /// <summary>
    /// Интерфейс Наблюдатель паттерна Наблюдатель
    /// </summary>
    internal interface ITimeObserver
    {
        /// <summary>
        /// Получение обновления от издателя
        /// </summary>
        /// <param name="subject">Издатель</param>
        void Update(ITime subject);
    }
}
