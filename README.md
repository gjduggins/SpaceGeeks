# SpaceGeeks

SpaceGeeks is an ASP.NET Core web application dedicated to space education, providing information about the planets in our solar system and historic NASA missions.

## Features

### Solar System Planets
Browse facts about every planet, including:
- Diameter and mass
- Distance from the Sun
- Number of moons
- Orbital period
- Planet imagery

### NASA Missions
Explore notable NASA missions:
- Apollo 11 — First crewed lunar landing
- Voyager 1 — Interstellar space probe
- Hubble Space Telescope — Orbital astronomy observatory
- Mars Rover Perseverance — Searching for signs of ancient life
- James Webb Space Telescope — Infrared observatory

## Technology Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8 Razor Pages |
| Language | C# 12 |
| UI | Bootstrap 5, HTML5/CSS3 |
| Testing | xunit, FsCheck (property-based), HtmlAgilityPack |

## Project Structure

```
SpaceGeeks/
├── SpaceGeeks/              # Main web application
│   ├── Models/              # Immutable records: Planet, NasaMission
│   ├── Data/                # Repository interfaces & in-memory implementations
│   ├── Pages/               # Razor Pages + shared partials
│   └── wwwroot/             # Static assets (CSS, JS, images)
├── SpaceGeeks.Tests/        # Unit and integration tests
└── Documentation/           # Delivery plans and technical specs
```

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Run the application

```bash
cd SpaceGeeks
dotnet run
```

Then open your browser to:
- **HTTP:** http://localhost:5233
- **HTTPS:** https://localhost:7066

### Run the tests

```bash
cd SpaceGeeks.Tests
dotnet test
```

## Documentation

Additional documentation lives in the `Documentation/` folder:
- Delivery plans
- Feature plans and enhancement proposals
- Implementation summaries

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/my-feature`)
3. Commit your changes
4. Push to the branch and open a pull request

## License

This project is licensed under the MIT License.
