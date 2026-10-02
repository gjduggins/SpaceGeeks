using Microsoft.AspNetCore.Mvc.RazorPages;
using SpaceGeeks.Data;
using SpaceGeeks.Models;

namespace SpaceGeeks.Pages;

public class StarsModel : PageModel
{
    private readonly IStarRepository _repo;

    public IReadOnlyList<Star> Stars { get; private set; } = Array.Empty<Star>();

    public StarsModel(IStarRepository repo)
    {
        _repo = repo;
    }

    public void OnGet()
    {
        Stars = _repo.GetAllOrderedByDistance();
    }
}