using SpaceGeeks.Models;

namespace SpaceGeeks.Data;

/// <summary>
/// Provides access to planet data.
/// </summary>
public interface IPlanetRepository
{
    /// <summary>Returns all planets ordered by ascending distance from the sun.</summary>
    IReadOnlyList<Planet> GetAllOrderedByDistance();
}
