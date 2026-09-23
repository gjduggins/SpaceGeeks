using SpaceGeeks.Models;

namespace SpaceGeeks.Data;

/// <summary>
/// Provides access to NASA mission data.
/// </summary>
public interface INasaMissionRepository
{
    /// <summary>Returns all NASA missions ordered by ascending launch date.</summary>
    IReadOnlyList<NasaMission> GetAllOrderedByLaunchDate();
}