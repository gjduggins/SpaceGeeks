# SpaceGeeks — Web Application

This is the main ASP.NET Core Razor Pages project for the SpaceGeeks application.

## Technology Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 Razor Pages |
| Language | C# 12 |
| UI | Bootstrap 5, HTML5/CSS3 |

## Project Structure

```
SpaceGeeks/
├── Models/         # Immutable records
│   ├── Planet.cs
│   └── NasaMission.cs
├── Data/           # Repository pattern
│   ├── IPlanetRepository.cs
│   ├── IINasaMissionRepository.cs
│   ├── InMemoryPlanetRepository.cs
│   └── InMemoryNasaMissionRepository.cs
├── Pages/          # Razor Pages
│   ├── Index.cshtml         # Home page
│   ├── NasaMissions.cshtml  # Missions listing
│   └── Shared/
│       ├── _Layout.cshtml
│       ├── _PlanetCard.cshtml
│       └── _MissionCard.cshtml
├── wwwroot/        # Static assets (CSS, JS, images)
└── Properties/
    └── launchSettings.json
```

## Running Locally

```bash
dotnet run
```

The app will be available at:
- **HTTP:** http://localhost:5233
- **HTTPS:** https://localhost:7066

## Models

**Planet** — immutable record with: `Name`, `DiameterKm`, `MassKg`, `DistanceFromSunKm`, `NumberOfMoons`, `OrbitalPeriodDays`, `ImagePath`

**NasaMission** — immutable record with: `Name`, `Description`, `LaunchDate`, `EndDate`, `Status`, `ImagePath`

## Data Layer

Both repositories follow a simple interface pattern with an in-memory implementation. To swap in a database-backed implementation, implement `IPlanetRepository` or `INasaMissionRepository` and register it in `Program.cs`.
