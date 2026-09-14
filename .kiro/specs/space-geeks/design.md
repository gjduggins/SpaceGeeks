# Design Document: SpaceGeeks

## Overview

SpaceGeeks is a read-only, single-page .NET Core web application that presents educational facts about the eight planets of the solar system. The first iteration delivers one page — the Planet_Page — which renders eight Planet_Cards ordered by ascending distance from the sun.

The application has no database, no user authentication, and no user-generated content. All planet data is stored as a static in-memory collection defined at startup. The primary design goals are:

- **Simplicity**: A minimal, maintainable codebase using standard ASP.NET Core conventions.
- **Performance**: Fast HTML responses (< 2 s under normal load) with proper caching and compression.
- **Responsiveness**: A CSS layout that adapts cleanly from 320 px to 2560 px.
- **Reliability**: Graceful error handling that never exposes internals to the user.

---

## Architecture

The application follows the **ASP.NET Core Razor Pages** pattern, which is well-suited to a read-only, single-page presentation site. Razor Pages co-locates the page model (data preparation) with its view template, reducing the number of files and indirection compared to full MVC for a site of this size.

```
┌─────────────────────────────────────────────────────────────┐
│                        Browser / User                       │
└────────────────────────────┬────────────────────────────────┘
                             │  HTTP GET /
                             ▼
┌─────────────────────────────────────────────────────────────┐
│               ASP.NET Core Middleware Pipeline               │
│  ┌──────────────┐  ┌───────────────┐  ┌───────────────────┐ │
│  │ Static Files │  │ Response      │  │ Exception Handler │ │
│  │ Middleware   │  │ Compression   │  │ Middleware        │ │
│  └──────────────┘  └───────────────┘  └───────────────────┘ │
└────────────────────────────┬────────────────────────────────┘
                             │
                             ▼
┌─────────────────────────────────────────────────────────────┐
│                   Razor Pages Router                        │
│                                                             │
│   Pages/Index.cshtml.cs  (IndexModel — PageModel)           │
│   Pages/Index.cshtml     (Razor template)                   │
└────────────────────────────┬────────────────────────────────┘
                             │  queries
                             ▼
┌─────────────────────────────────────────────────────────────┐
│                    IPlanetRepository                        │
│          (interface + static in-memory implementation)      │
└─────────────────────────────────────────────────────────────┘
```

### Key Architectural Decisions

| Decision | Choice | Rationale |
|---|---|---|
| Web framework | Razor Pages | Minimal ceremony for a single read-only page |
| Data layer | Static in-memory list | No database required; data is fixed facts |
| Templating | Razor partial view `_PlanetCard.cshtml` | Encapsulates card markup, keeps Index clean |
| Compression | `AddResponseCompression()` middleware | Satisfies gzip/brotli requirement with zero custom code |
| Static asset caching | `UseStaticFiles()` with `StaticFileOptions` | Sets `Cache-Control: max-age=3600` globally for static assets |
| Error handling | `UseExceptionHandler("/Error")` | Renders a friendly page; never exposes stack traces |

---

## Components and Interfaces

### 1. `IPlanetRepository`

```csharp
public interface IPlanetRepository
{
    /// <summary>Returns all planets ordered by ascending distance from the sun.</summary>
    IReadOnlyList<Planet> GetAllOrderedByDistance();
}
```

### 2. `InMemoryPlanetRepository`

Implements `IPlanetRepository`. Holds a static, hardcoded `IReadOnlyList<Planet>` initialised once at class load time. Planets are stored in ascending distance order so the sort is essentially free.

```csharp
public sealed class InMemoryPlanetRepository : IPlanetRepository
{
    private static readonly IReadOnlyList<Planet> _planets = new List<Planet>
    {
        new Planet("Mercury", diameterKm: 4_879, massKg: 3.285e23, ...),
        // ... Venus, Earth, Mars, Jupiter, Saturn, Uranus, Neptune
    }.AsReadOnly();

    public IReadOnlyList<Planet> GetAllOrderedByDistance() => _planets;
}
```

### 3. `IndexModel` (Razor Page Model)

Located at `Pages/Index.cshtml.cs`. Retrieves the planet list from `IPlanetRepository` on `OnGet()` and exposes it as a property for the template.

```csharp
public class IndexModel : PageModel
{
    private readonly IPlanetRepository _repo;
    public IReadOnlyList<Planet> Planets { get; private set; } = Array.Empty<Planet>();

    public IndexModel(IPlanetRepository repo) { _repo = repo; }

    public void OnGet() => Planets = _repo.GetAllOrderedByDistance();
}
```

### 4. `Pages/Index.cshtml`

Razor template that iterates over `Model.Planets` and renders `<partial name="_PlanetCard" model="planet" />` for each one inside a CSS grid container.

### 5. `Pages/Shared/_PlanetCard.cshtml`

Partial view that receives a `Planet` model and renders:
- Planet name (`<h2>`)
- `<img>` with `alt` text and `onerror` handler for graceful fallback
- Fact list: diameter, mass, distance from sun, moon count, orbital period

### 6. `Pages/Error.cshtml`

User-friendly error page. Displays a heading and brief message. Never exposes `RequestId` in stack trace form, `Exception.Message`, or `Exception.StackTrace`.

### 7. Middleware Registration (`Program.cs`)

```csharp
builder.Services.AddResponseCompression(opts =>
{
    opts.EnableForHttps = true;
    opts.Providers.Add<BrotliCompressionProvider>();
    opts.Providers.Add<GzipCompressionProvider>();
});
builder.Services.AddSingleton<IPlanetRepository, InMemoryPlanetRepository>();

// ...

app.UseExceptionHandler("/Error");
app.UseResponseCompression();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
        ctx.Context.Response.Headers.CacheControl = "public, max-age=31536000, immutable"
});
app.UseRouting();
app.MapRazorPages();
```

---

## Data Models

### `Planet`

```csharp
public sealed record Planet(
    string Name,
    double DiameterKm,
    double MassKg,
    double DistanceFromSunKm,
    int NumberOfMoons,
    double OrbitalPeriodDays,
    string ImagePath          // relative URL to static asset, e.g. "/images/mercury.webp"
);
```

All fields are immutable (record). `ImagePath` points to a static file served by the application; the partial view renders it in an `<img>` tag with an `onerror` JavaScript fallback to a placeholder image.

### Planet Data (Static Values)

| Planet | Diameter (km) | Mass (kg) | Distance from Sun (km) | Moons | Orbital Period (days) |
|---|---|---|---|---|---|
| Mercury | 4,879 | 3.285 × 10²³ | 57,900,000 | 0 | 88.0 |
| Venus | 12,104 | 4.867 × 10²⁴ | 108,200,000 | 0 | 224.7 |
| Earth | 12,756 | 5.972 × 10²⁴ | 149,600,000 | 1 | 365.2 |
| Mars | 6,779 | 6.390 × 10²³ | 227,900,000 | 2 | 687.0 |
| Jupiter | 139,820 | 1.898 × 10²⁷ | 778,500,000 | 95 | 4,333.0 |
| Saturn | 116,460 | 5.683 × 10²⁶ | 1,432,000,000 | 146 | 10,759.0 |
| Uranus | 50,724 | 8.681 × 10²⁵ | 2,867,000,000 | 28 | 30,687.0 |
| Neptune | 49,244 | 1.024 × 10²⁶ | 4,515,000,000 | 16 | 60,190.0 |

---

## Correctness Properties

*A property is a characteristic or behavior that should hold true across all valid executions of a system — essentially, a formal statement about what the system should do. Properties serve as the bridge between human-readable specifications and machine-verifiable correctness guarantees.*

The following properties were derived from the acceptance criteria. CSS layout and browser rendering requirements (Requirement 2) are not suitable for property-based testing and are instead covered by integration/visual tests. Infrastructure and performance requirements that don't vary meaningfully with input (1.2, 3.1, 3.3, 4.1–4.4) are covered by smoke and integration tests.

---

### Property 1: All planets have complete fact data

*For any* planet returned by the repository, the planet record SHALL have a non-empty name, a positive diameter, a positive mass, a positive distance from the sun, a non-negative moon count, a positive orbital period, and a non-empty image path.

**Validates: Requirements 1.3**

---

### Property 2: Planets are ordered by ascending distance from the sun

*For any* consecutive pair of planets in the list returned by the repository, the earlier planet's distance from the sun SHALL be strictly less than the later planet's distance from the sun.

**Validates: Requirements 1.4**

---

### Property 3: Static assets carry a sufficient Cache-Control max-age header

*For any* static asset served by the application (CSS, JavaScript, or image file), the `Cache-Control` response header SHALL include a `max-age` value of at least 3600 seconds.

**Validates: Requirements 3.2**

---

### Property 4: Error responses never expose exception internals

*For any* unhandled exception raised during a request, the HTML response body returned by the error page SHALL NOT contain stack trace indicators (e.g., substrings " at ", "Exception", or Windows/Unix file path separators that indicate a call stack), exception type names, or raw exception messages.

**Validates: Requirements 4.5**

---

## Error Handling

### Unhandled Exceptions

ASP.NET Core's built-in `UseExceptionHandler("/Error")` middleware is registered in `Program.cs`. Any unhandled exception in a Razor Page handler:
1. Is caught by the middleware.
2. Triggers a redirect (internal rewrite) to `GET /Error`.
3. Returns HTTP 500 to the client.
4. Renders `Pages/Error.cshtml` — a static, safe page with a heading and short message.

`Pages/Error.cshtml` explicitly does **not** read `HttpContext.Features.Get<IExceptionHandlerPathFeature>()` in production mode. The `ASPNETCORE_ENVIRONMENT` is expected to be `Production` when deployed; in `Development` the developer exception page may be shown instead (standard ASP.NET Core pattern).

### Image Load Failures

Planet card images are served as static files. If an image fails to load in the browser, the `<img>` tag uses an `onerror` JavaScript attribute to swap in a placeholder SVG bundled with the application:

```html
<img src="@Model.ImagePath"
     alt="@Model.Name"
     onerror="this.onerror=null; this.src='/images/placeholder.svg';" />
```

This satisfies Requirement 1.5 without any server-side logic.

### Missing / Unknown Routes

Any request to an unknown URL returns a standard 404 Not Found response via Razor Pages' default routing. No custom 404 page is required in this iteration.

---

## Testing Strategy

### Overview

The testing strategy uses two complementary layers:
- **Unit/property tests**: Fast, in-process tests of the data layer and business logic.
- **Integration tests**: HTTP-level tests verifying middleware configuration, routing, compression, caching, and error handling.

Browser-based CSS layout tests (Requirement 2) are deferred to manual QA or a future visual regression suite.

### Unit & Property Tests

**Framework**: xUnit + [FsCheck](https://fscheck.github.io/FsCheck/) (property-based testing library for .NET)

All property tests are configured to run a **minimum of 100 iterations**.

#### Property Test: All planets have complete fact data

Tag: `Feature: space-geeks, Property 1: All planets have complete fact data`

- Instantiate `InMemoryPlanetRepository`.
- Call `GetAllOrderedByDistance()`.
- For each planet in the result, assert:
  - `Name` is non-null and non-empty
  - `DiameterKm > 0`
  - `MassKg > 0`
  - `DistanceFromSunKm > 0`
  - `NumberOfMoons >= 0`
  - `OrbitalPeriodDays > 0`
  - `ImagePath` is non-null and non-empty

#### Property Test: Planets are ordered by ascending distance

Tag: `Feature: space-geeks, Property 2: Planets are ordered by ascending distance from the sun`

- Instantiate `InMemoryPlanetRepository`.
- Call `GetAllOrderedByDistance()`.
- Iterate consecutive pairs and assert `planets[i].DistanceFromSunKm < planets[i+1].DistanceFromSunKm`.

#### Property Test: Error responses never expose exception internals

Tag: `Feature: space-geeks, Property 4: Error responses never expose exception internals`

- Using FsCheck, generate arbitrary exception messages (random strings of varying length and character content).
- For each generated message, trigger a simulated 500 response via `WebApplicationFactory<Program>`.
- Assert response body does not contain any of: `" at "`, `"Exception"`, `"\\"`, `"/"` preceded by a drive letter pattern.

### Integration Tests

**Framework**: xUnit + `Microsoft.AspNetCore.Mvc.Testing` (`WebApplicationFactory<Program>`)

| Test | What is verified | Requirement |
|---|---|---|
| `GET /` returns HTTP 200 | Route wired up correctly | 1.2 |
| `GET /` response contains 8 planet names | All cards rendered | 1.1 |
| `GET /images/*.webp` has `Cache-Control: max-age >= 3600` | Static file caching | 3.2 |
| `GET /css/*.css` with `Accept-Encoding: gzip` returns `Content-Encoding: gzip` | Compression middleware | 3.3 |
| `GET /` responds in < 2000 ms | Page performance | 3.1 |
| Error endpoint returns HTTP 500 and body without stack trace text | Error handling | 4.4, 4.5 |

#### Property Integration Test: Static assets carry sufficient Cache-Control header

Tag: `Feature: space-geeks, Property 3: Static assets carry a sufficient Cache-Control max-age header`

- Enumerate a representative set of static asset paths (CSS, JS, image files).
- For each path, send `GET {path}` via `WebApplicationFactory`.
- Parse the `Cache-Control` header and assert `max-age >= 3600`.

### Unit Tests (Example-Based)

| Test | Validates |
|---|---|
| `InMemoryPlanetRepository` returns exactly 8 planets | 1.1 |
| Planet names match expected set {Mercury … Neptune} | 1.1 |
| First planet is Mercury, last is Neptune | 1.4 |
| `onerror` attribute present on all `<img>` tags in rendered HTML | 1.5 |

### Not Covered by Automated Tests (Manual QA)

- Responsive layout breakpoints (320 px, 768 px, 1280 px, 2560 px) — Requirement 2.1–2.5
- Visual appearance of Planet_Cards, image rendering
- Cross-browser compatibility
