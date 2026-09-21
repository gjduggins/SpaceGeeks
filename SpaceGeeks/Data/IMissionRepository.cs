using SpaceGeeks.Models;

namespace SpaceGeeks.Data;

/// <summary>
/// Provides access to NASA mission data.
/// </summary>
public interface IMissionRepository
{
    /// <summary>Returns all missions ordered by launch date.</summary>
    IReadOnlyList<Mission> GetAllOrderedByLaunchDate();
}