namespace SpaceGeeks.Models;

/// <summary>
/// Represents a planet in the solar system with its key facts.
/// This is an immutable record — all properties are set at construction time.
/// </summary>
public sealed record Planet(
    string Name,
    double DiameterKm,
    double MassKg,
    double DistanceFromSunKm,
    int NumberOfMoons,
    double OrbitalPeriodDays,
    string ImagePath          // relative URL to static asset, e.g. "/images/mercury.webp"
);
