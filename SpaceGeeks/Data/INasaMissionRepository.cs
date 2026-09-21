using SpaceGeeks.Models;

namespace SpaceGeeks.Data;

/// <summary>
/// Repository interface for accessing NASA mission data.
/// </summary>
public interface INasaMissionRepository
{
    /// <summary>
    /// Gets all NASA missions ordered by year.
    /// </summary>
    /// <returns>A read-only list of NASA missions ordered by year.</returns>
    IReadOnlyList<NasaMission> GetAllOrderedByYear();
}