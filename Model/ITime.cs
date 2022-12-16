﻿namespace TrafficModeling.Model
{
    internal interface ITime
    {
        // Присоединяет наблюдателя к издателю.
        void Attach(ITimeObserver observer);

        // Отсоединяет наблюдателя от издателя.
        void Detach(ITimeObserver observer);

        // Уведомляет всех наблюдателей о событии.
        void Notify();
    }
}