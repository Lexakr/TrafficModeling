namespace TrafficModeling.Model
{
    /// <summary>
    /// Гражданский транспорт без приоритета.
    /// </summary>
    internal class CivilCar : Car
    {
        public CivilCar(string Name, int Speed, string Origin, int arrivalTime) : base(Name, Speed, Origin, arrivalTime)
        {
            Priority = false;
        }
    }
}
