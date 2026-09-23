using Microsoft.AspNetCore.Mvc.RazorPages;
using SpaceGeeks.Data;
using SpaceGeeks.Models;

namespace SpaceGeeks.Pages;

public class NasaMissionsModel : PageModel
{
    private readonly INasaMissionRepository _repo;

    public IReadOnlyList<NasaMission> Missions { get; private set; } = Array.Empty<NasaMission>();
    
    [BindProperty(SupportsGet = true)]
    public string? StatusFilter { get; set; }

    public NasaMissionsModel(INasaMissionRepository repo)
    {
        _repo = repo;
    }

    public void OnGet()
    {
        var allMissions = _repo.GetAllOrderedByLaunchDate();
        
        if (string.IsNullOrWhiteSpace(StatusFilter))
        {
            Missions = allMissions;
        }
        else
        {
            Missions = allMissions
                .Where(m => m.Status.Equals(StatusFilter, StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }
    }
}