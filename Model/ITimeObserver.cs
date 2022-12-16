namespace TrafficModeling.Model
{
    internal interface ITimeObserver
    {
        // Получение обновления от издателя
        void Update(ITime subject);
    }
}
