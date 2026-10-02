namespace SpaceGeeks.Models
{
    public class Star
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public double DistanceFromEarthLightYears { get; set; }
        public string Luminosity { get; set; }
        public int TemperatureKelvin { get; set; }
    }
}