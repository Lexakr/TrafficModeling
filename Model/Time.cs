using System.Collections.Generic;

namespace TrafficModeling.Model
{

    internal class Time : ITime
    {
        private int currentTime = 0;
        private List<ITimeObserver> _observers = new();
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

        public void Increment()
        {
            currentTime++;
            Notify();
        }
    }
}
