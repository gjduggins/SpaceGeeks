using Microsoft.AspNetCore.Mvc.RazorPages;
using SpaceGeeks.Data;
using SpaceGeeks.Models;

namespace SpaceGeeks.Pages
{
    public class StarsModel : PageModel
    {
        private readonly IStarRepository _starRepository;

        public StarsModel(IStarRepository starRepository)
        {
            _starRepository = starRepository;
        }

        public IEnumerable<Star> Stars { get; set; } = new List<Star>();

        public void OnGet()
        {
            Stars = _starRepository.GetAll();
        }
    }
}