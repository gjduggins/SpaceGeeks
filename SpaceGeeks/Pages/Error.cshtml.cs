using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SpaceGeeks.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    public void OnGet()
    {
        // Intentionally empty — no exception details, request IDs, or
        // stack trace information are read or exposed to the view.
    }
}
