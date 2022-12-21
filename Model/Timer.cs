using System.Collections.Generic;

namespace TrafficModeling.Model
{
    /// <summary>
    /// Таймер симуляции. Издатель паттерна Наблюдатель.
    /// </summary>
    internal class Timer : ITime
    {
        /// <summary>
        /// Коллекция наблюдателей
        /// </summary>
        private readonly List<ITimeObserver> _observers = new();

        /// <summary>
        /// Текущее время в тиках
        /// </summary>
        private int currentTime = 0;

        /// <summary>
        /// Текущее время в тиках
        /// </summary>
        public int CurrentTime { get { return currentTime; } }

        public void Attach(ITimeObserver observer) => _observers.Add(observer);
        public void Detach(ITimeObserver observer) => _observers.Remove(observer);
        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(this);
            }
        }

        /// <summary>
        /// Текущее время + 1 тик.
        /// </summary>
        public void Increment()
        {
            currentTime++;
            Notify();
        }
    }
}
