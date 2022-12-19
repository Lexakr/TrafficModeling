namespace TrafficModeling.Model
{
    internal class GovCar : Car
    {
        public GovCar(string Name, int Speed, string Origin, int arrivalTime) : base(Name, Speed, Origin, arrivalTime)
        {
            Priority = true;
        }
    }
}
