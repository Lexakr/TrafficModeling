namespace TrafficModeling.Model
{
    /// <summary>
    /// Транспорт спецслужб с приоритетом.
    /// </summary>
    internal class GovCar : Car
    {
        public GovCar(string Name, int Speed, string Origin, int arrivalTime) : base(Name, Speed, Origin, arrivalTime)
        {
            Priority = true;
        }
    }
}
