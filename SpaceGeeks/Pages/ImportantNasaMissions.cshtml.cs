using Microsoft.AspNetCore.Mvc.RazorPages;
using SpaceGeeks.Data;
using SpaceGeeks.Models;

namespace SpaceGeeks.Pages;

public class ImportantNasaMissionsModel : PageModel
{
    private readonly INasaMissionRepository _repo;

    public IReadOnlyList<NasaMission> Missions { get; private set; } = Array.Empty<NasaMission>();

    public ImportantNasaMissionsModel(INasaMissionRepository repo)
    {
        _repo = repo;
    }

    public void OnGet()
    {
        var allMissions = _repo.GetAllOrderedByLaunchDate();
        
        // Filter for important missions based on historical significance
        Missions = allMissions.Where(IsImportantMission).ToArray();
    }

    /// <summary>
    /// Determines if a mission is considered historically important based on its impact and significance.
    /// </summary>
    /// <param name="mission">The NASA mission to evaluate</param>
    /// <returns>True if the mission is considered important, false otherwise</returns>
    private static bool IsImportantMission(NasaMission mission)
    {
        // Define important missions based on historical significance
        var importantMissionNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Apollo 11",              // First moon landing
            "Voyager 1",              // Interstellar space exploration
            "Hubble Space Telescope"  // Revolutionary space astronomy
        };

        return importantMissionNames.Contains(mission.Name);
    }
}