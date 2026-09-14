# Implementation Plan: SpaceGeeks

## Overview

Build a read-only ASP.NET Core Razor Pages application that displays educational facts about the eight solar system planets. The implementation follows a bottom-up approach: define the data model and repository first, wire up the Razor Pages layer, add static assets, configure middleware, and then cover correctness with property and integration tests.

## Tasks

- [x] 1. Set up project structure and core data model
  - Create a new ASP.NET Core Razor Pages project targeting .NET 8 (`SpaceGeeks.csproj`)
  - Define the `Planet` immutable record in `Models/Planet.cs`
  - Add xUnit and FsCheck NuGet references to the test project (`SpaceGeeks.Tests.csproj`)
  - _Requirements: 4.1, 4.2_

- [x] 2. Implement the data layer
  - [x] 2.1 Define `IPlanetRepository` interface
    - Create `Data/IPlanetRepository.cs` with the `GetAllOrderedByDistance()` method signature returning `IReadOnlyList<Planet>`
    - _Requirements: 4.3_
  - [x] 2.2 Implement `InMemoryPlanetRepository`
    - Create `Data/InMemoryPlanetRepository.cs` with a static readonly list containing all eight planets in ascending distance order, using the exact fact values from the design
    - _Requirements: 1.1, 1.3, 1.4, 4.3_
  - [ ]* 2.3 Write property test for complete planet data (Property 1)
    - **Property 1: All planets have complete fact data**
    - Instantiate `InMemoryPlanetRepository`, call `GetAllOrderedByDistance()`, assert each planet has non-empty name, positive diameter, positive mass, positive distance, non-negative moon count, positive orbital period, and non-empty image path
    - Run minimum 100 iterations via FsCheck
    - **Validates: Requirements 1.3**
  - [ ]* 2.4 Write property test for ascending distance ordering (Property 2)
    - **Property 2: Planets are ordered by ascending distance from the sun**
    - Instantiate `InMemoryPlanetRepository`, iterate consecutive pairs, assert `planets[i].DistanceFromSunKm < planets[i+1].DistanceFromSunKm`
    - Run minimum 100 iterations via FsCheck
    - **Validates: Requirements 1.4**
  - [ ]* 2.5 Write unit tests for `InMemoryPlanetRepository`
    - Assert exactly 8 planets are returned
    - Assert planet names match `{ Mercury, Venus, Earth, Mars, Jupiter, Saturn, Uranus, Neptune }`
    - Assert Mercury is first and Neptune is last
    - _Requirements: 1.1, 1.4_

- [ ] 3. Checkpoint — Ensure all data-layer tests pass
  - Ensure all tests pass, ask the user if questions arise.

- [x] 4. Implement Razor Pages layer
  - [x] 4.1 Implement `Pages/Index.cshtml.cs` (`IndexModel`)
    - Inject `IPlanetRepository` via constructor, expose `IReadOnlyList<Planet> Planets` property, populate it in `OnGet()`
    - _Requirements: 1.2, 1.4_
  - [x] 4.2 Implement `Pages/Shared/_PlanetCard.cshtml` partial view
    - Render planet name (`<h2>`), `<img>` with `alt` text and `onerror` fallback to `/images/placeholder.svg`, and a fact list: diameter, mass, distance from sun, moon count, orbital period
    - _Requirements: 1.3, 1.5_
  - [x] 4.3 Implement `Pages/Index.cshtml` index page
    - Iterate `Model.Planets` inside a CSS grid container and render `<partial name="_PlanetCard" model="planet" />` for each planet
    - _Requirements: 1.1, 1.4, 2.1, 2.2, 2.3, 2.4_
  - [x] 4.4 Implement `Pages/Error.cshtml` error page
    - Display a user-readable heading and brief message; explicitly exclude stack traces, exception type names, and raw exception messages
    - _Requirements: 4.4, 4.5_
  - [x] 4.5 Write unit test for `onerror` attribute presence
    - Render the Index page via `WebApplicationFactory`, parse the HTML, assert every `<img>` tag inside a planet card has a non-empty `onerror` attribute
    - _Requirements: 1.5_

- [x] 5. Add static assets and responsive CSS
  - [x] 5.1 Add planet images and placeholder SVG
    - Place eight planet `.webp` images and `placeholder.svg` into `wwwroot/images/`; set each `Planet.ImagePath` in the repository to the corresponding `/images/{name}.webp` URL
    - _Requirements: 1.3, 1.5_
  - [x] 5.2 Add responsive CSS stylesheet
    - Create `wwwroot/css/site.css` implementing a mobile-first CSS grid: single-column below 768 px, at least two columns from 768 px, at least three columns from 1280 px; ensure text wraps and no horizontal scrollbar appears between 320 px and 2560 px
    - _Requirements: 2.1, 2.2, 2.3, 2.4, 2.5_

- [ ] 6. Configure middleware in `Program.cs`
  - Register `AddResponseCompression()` with Brotli and Gzip providers
  - Register `AddSingleton<IPlanetRepository, InMemoryPlanetRepository>()`
  - Register Razor Pages with `AddRazorPages()`
  - Configure `UseExceptionHandler("/Error")`, `UseResponseCompression()`, `UseStaticFiles()` with `Cache-Control: public, max-age=31536000, immutable`, `UseRouting()`, and `MapRazorPages()`
  - _Requirements: 3.2, 3.3, 4.1, 4.4_

- [ ] 7. Checkpoint — Ensure application builds and smoke-tests pass
  - Ensure all tests pass, ask the user if questions arise.

- [ ] 8. Write integration and property integration tests
  - [ ] 8.1 Write integration test: `GET /` returns HTTP 200 and 8 planet names
    - Use `WebApplicationFactory<Program>`; assert HTTP 200 and that all eight planet names appear in the response body
    - _Requirements: 1.1, 1.2_
  - [ ] 8.2 Write integration test: response time under 2000 ms
    - Time a `GET /` request via `WebApplicationFactory`; assert elapsed time < 2000 ms
    - _Requirements: 3.1_
  - [ ] 8.3 Write integration test: gzip/brotli compression
    - Send `GET /css/site.css` with `Accept-Encoding: gzip`; assert `Content-Encoding: gzip` in the response headers
    - _Requirements: 3.3_
  - [ ]* 8.4 Write property integration test for static asset caching (Property 3)
    - **Property 3: Static assets carry a sufficient Cache-Control max-age header**
    - Enumerate representative static asset paths (CSS, JS, image files); for each path `GET {path}` via `WebApplicationFactory`; parse `Cache-Control` header and assert `max-age >= 3600`
    - Run minimum 100 iterations via FsCheck
    - **Validates: Requirements 3.2**
  - [ ]* 8.5 Write property integration test for safe error responses (Property 4)
    - **Property 4: Error responses never expose exception internals**
    - Generate arbitrary exception messages via FsCheck; trigger a simulated 500 response via `WebApplicationFactory<Program>`; assert response body does not contain `" at "`, `"Exception"`, or file-path separators indicating a call stack
    - Run minimum 100 iterations via FsCheck
    - **Validates: Requirements 4.5**

- [ ] 9. Final checkpoint — Ensure all tests pass
  - Ensure all tests pass, ask the user if questions arise.

## Notes

- Tasks marked with `*` are optional and can be skipped for a faster MVP
- Each task references specific requirements for traceability
- Property tests use FsCheck with a minimum of 100 iterations each
- Unit tests cover example-based cases complementing the property tests
- Responsive layout (Requirement 2) requires manual QA for visual breakpoints; automated tests cover structural HTML output only
- Planet images need to be sourced and converted to `.webp` format before task 5.1 can be completed

## Task Dependency Graph

```json
{
  "waves": [
    { "id": 0, "tasks": ["2.1"] },
    { "id": 1, "tasks": ["2.2"] },
    { "id": 2, "tasks": ["2.3", "2.4", "2.5"] },
    { "id": 3, "tasks": ["4.1", "4.2", "5.1", "5.2"] },
    { "id": 4, "tasks": ["4.3", "4.4"] },
    { "id": 5, "tasks": ["4.5", "6"] },
    { "id": 6, "tasks": ["8.1", "8.2", "8.3"] },
    { "id": 7, "tasks": ["8.4", "8.5"] }
  ]
}
```
