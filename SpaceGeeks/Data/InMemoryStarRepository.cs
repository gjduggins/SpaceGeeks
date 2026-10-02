using System.Collections.Generic;
using SpaceGeeks.Models;

namespace SpaceGeeks.Data
{
    public class InMemoryStarRepository : IStarRepository
    {
        public IReadOnlyList<Star> GetAllOrderedByDistance()
        {
            return new List<Star>
            {
                new Star { Name = "Sun", ImagePath = "/images/sun.webp", DistanceFromEarthLightYears = 0, Luminosity = "1", TemperatureKelvin = 5778 },
                new Star { Name = "Proxima Centauri", ImagePath = "/images/proxima-centauri.webp", DistanceFromEarthLightYears = 4.24, Luminosity = "0.0017", TemperatureKelvin = 3042 },
                new Star { Name = "Sirius", ImagePath = "/images/sirius.webp", DistanceFromEarthLightYears = 8.6, Luminosity = "25.4", TemperatureKelvin = 9940 },
                new Star { Name = "Betelgeuse", ImagePath = "/images/betelgeuse.webp", DistanceFromEarthLightYears = 642.5, Luminosity = "126000", TemperatureKelvin = 3500 },
                new Star { Name = "Vega", ImagePath = "/images/vega.webp", DistanceFromEarthLightYears = 25.04, Luminosity = "40.12", TemperatureKelvin = 9602 }
            };
        }
    }
}