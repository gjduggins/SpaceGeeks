using Microsoft.AspNetCore.Mvc.RazorPages;
using SpaceGeeks.Data;
using SpaceGeeks.Models;

namespace SpaceGeeks.Pages;

public class NasaMissionsModel : PageModel
{
    private readonly INasaMissionRepository _repo;

    public IReadOnlyList<NasaMission> Missions { get; private set; } = Array.Empty<NasaMission>();

    public NasaMissionsModel(INasaMissionRepository repo)
    {
        _repo = repo;
    }

    public void OnGet()
    {
        Missions = _repo.GetAllOrderedByLaunchDate();
    }
}