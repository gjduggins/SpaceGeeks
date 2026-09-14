using Microsoft.AspNetCore.Mvc.RazorPages;
using SpaceGeeks.Data;
using SpaceGeeks.Models;

namespace SpaceGeeks.Pages;

public class IndexModel : PageModel
{
    private readonly IPlanetRepository _repo;

    public IReadOnlyList<Planet> Planets { get; private set; } = Array.Empty<Planet>();

    public IndexModel(IPlanetRepository repo)
    {
        _repo = repo;
    }

    public void OnGet()
    {
        Planets = _repo.GetAllOrderedByDistance();
    }
}
