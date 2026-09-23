using SpaceGeeks.Models;

namespace SpaceGeeks.Data;

/// <summary>
/// Provides access to NASA mission data from an in-memory collection.
/// </summary>
public sealed class InMemoryNasaMissionRepository : INasaMissionRepository
{
    private static readonly IReadOnlyList<NasaMission> _missions = new[]
    {
        new NasaMission(
            "Apollo 11",
            "First crewed mission to land on the Moon",
            new DateTime(1969, 7, 16),
            new DateTime(1969, 7, 24),
            "Completed",
            "/images/apollo11.webp"
        ),
        new NasaMission(
            "Voyager 1",
            "Space probe studying interstellar space",
            new DateTime(1977, 9, 5),
            null,
            "Active",
            "/images/voyager1.webp"
        ),
        new NasaMission(
            "Hubble Space Telescope",
            "Space telescope for astronomy observations",
            new DateTime(1990, 4, 24),
            null,
            "Active",
            "/images/hubble.webp"
        ),
        new NasaMission(
            "Mars Rover Perseverance",
            "Mars rover searching for signs of ancient life",
            new DateTime(2020, 7, 30),
            null,
            "Active",
            "/images/perseverance.webp"
        ),
        new NasaMission(
            "James Webb Space Telescope",
            "Infrared space telescope",
            new DateTime(2021, 12, 25),
            null,
            "Active",
            "/images/webb.webp"
        )
    };

    /// <inheritdoc />
    public IReadOnlyList<NasaMission> GetAllOrderedByLaunchDate() =>
        _missions.OrderBy(m => m.LaunchDate).ToArray();
}