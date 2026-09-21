using Microsoft.AspNetCore.Mvc.RazorPages;
using SpaceGeeks.Data;
using SpaceGeeks.Models;

namespace SpaceGeeks.Pages;

public class MissionsModel : PageModel
{
    private readonly IMissionRepository _repo;

    public IReadOnlyList<Mission> Missions { get; private set; } = Array.Empty<Mission>();

    public MissionsModel(IMissionRepository repo)
    {
        _repo = repo;
    }

    public void OnGet()
    {
        Missions = _repo.GetAllOrderedByLaunchDate();
    }
}