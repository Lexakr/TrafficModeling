namespace TrafficModeling.Model
{
    internal class GovCar : Car
    {
        public GovCar(string Name, int Speed, string Origin) : base(Name, Speed, Origin)
        {
            Priority = true;
        }
    }
}
