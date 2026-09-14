using SpaceGeeks.Models;

namespace SpaceGeeks.Data;

/// <summary>
/// In-memory implementation of <see cref="IPlanetRepository"/>.
/// Planet data is stored as a static, hardcoded list initialised once at class load time,
/// ordered by ascending distance from the sun.
/// </summary>
public sealed class InMemoryPlanetRepository : IPlanetRepository
{
    private static readonly IReadOnlyList<Planet> _planets = new List<Planet>
    {
        new Planet("Mercury", DiameterKm: 4_879,    MassKg: 3.285e23, DistanceFromSunKm: 57_900_000,     NumberOfMoons: 0,   OrbitalPeriodDays: 88.0,    ImagePath: "/images/mercury.webp"),
        new Planet("Venus",   DiameterKm: 12_104,   MassKg: 4.867e24, DistanceFromSunKm: 108_200_000,    NumberOfMoons: 0,   OrbitalPeriodDays: 224.7,   ImagePath: "/images/venus.webp"),
        new Planet("Earth",   DiameterKm: 12_756,   MassKg: 5.972e24, DistanceFromSunKm: 149_600_000,    NumberOfMoons: 1,   OrbitalPeriodDays: 365.2,   ImagePath: "/images/earth.webp"),
        new Planet("Mars",    DiameterKm: 6_779,    MassKg: 6.390e23, DistanceFromSunKm: 227_900_000,    NumberOfMoons: 2,   OrbitalPeriodDays: 687.0,   ImagePath: "/images/mars.webp"),
        new Planet("Jupiter", DiameterKm: 139_820,  MassKg: 1.898e27, DistanceFromSunKm: 778_500_000,    NumberOfMoons: 95,  OrbitalPeriodDays: 4333.0,  ImagePath: "/images/jupiter.webp"),
        new Planet("Saturn",  DiameterKm: 116_460,  MassKg: 5.683e26, DistanceFromSunKm: 1_432_000_000,  NumberOfMoons: 146, OrbitalPeriodDays: 10759.0, ImagePath: "/images/saturn.webp"),
        new Planet("Uranus",  DiameterKm: 50_724,   MassKg: 8.681e25, DistanceFromSunKm: 2_867_000_000,  NumberOfMoons: 28,  OrbitalPeriodDays: 30687.0, ImagePath: "/images/uranus.webp"),
        new Planet("Neptune", DiameterKm: 49_244,   MassKg: 1.024e26, DistanceFromSunKm: 4_515_000_000,  NumberOfMoons: 16,  OrbitalPeriodDays: 60190.0, ImagePath: "/images/neptune.webp"),
    }.AsReadOnly();

    /// <inheritdoc />
    public IReadOnlyList<Planet> GetAllOrderedByDistance() => _planets;
}
