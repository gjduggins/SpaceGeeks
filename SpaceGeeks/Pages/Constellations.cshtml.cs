using Microsoft.AspNetCore.Mvc.RazorPages;
using SpaceGeeks.Data;
using SpaceGeeks.Models;

namespace SpaceGeeks.Pages
{
    public class ConstellationsModel : PageModel
    {
        private readonly IConstellationRepository _constellationRepository;

        public ConstellationsModel(IConstellationRepository constellationRepository)
        {
            _constellationRepository = constellationRepository;
        }

        public IEnumerable<Constellation> Constellations { get; set; } = new List<Constellation>();

        public void OnGet()
        {
            Constellations = _constellationRepository.GetAll();
        }
    }
}