using SpaceGeeks.Models;

namespace SpaceGeeks.Data;

/// <summary>
/// In-memory implementation of <see cref="IMissionRepository"/>.
/// NASA mission data is stored as a static, hardcoded list initialised once at class load time,
/// ordered by launch date.
/// </summary>
public sealed class InMemoryMissionRepository : IMissionRepository
{
    private static readonly IReadOnlyList<Mission> _missions = new List<Mission>
    {
        new Mission(
            Name: "Apollo 11",
            Description: "First crewed mission to land on the Moon. Neil Armstrong and Buzz Aldrin became the first humans to walk on the lunar surface.",
            LaunchDate: new DateTime(1969, 7, 16),
            LandingDate: new DateTime(1969, 7, 24),
            Successful: true,
            ImagePath: "/images/earth.webp"
        ),
        new Mission(
            Name: "Voyager 1",
            Description: "Space probe launched to study Jupiter and Saturn, now the farthest human-made object from Earth.",
            LaunchDate: new DateTime(1977, 9, 5),
            LandingDate: null,
            Successful: true,
            ImagePath: "/images/jupiter.webp"
        ),
        new Mission(
            Name: "Hubble Space Telescope",
            Description: "Large space telescope that has provided unprecedented deep-field images of distant galaxies and nebulae.",
            LaunchDate: new DateTime(1990, 4, 24),
            LandingDate: null,
            Successful: true,
            ImagePath: "/images/neptune.webp"
        ),
        new Mission(
            Name: "Mars Pathfinder",
            Description: "Demonstrated a low-cost way of delivering a rover to the surface of Mars, including the Sojourner rover.",
            LaunchDate: new DateTime(1996, 12, 4),
            LandingDate: new DateTime(1997, 7, 4),
            Successful: true,
            ImagePath: "/images/mars.webp"
        ),
        new Mission(
            Name: "International Space Station",
            Description: "Modular space station in low Earth orbit, a collaborative project between multiple space agencies.",
            LaunchDate: new DateTime(1998, 11, 20),
            LandingDate: null,
            Successful: true,
            ImagePath: "/images/earth.webp"
        ),
        new Mission(
            Name: "Spirit and Opportunity Rovers",
            Description: "Two robotic rovers that explored Mars, far exceeding their planned 90-day missions.",
            LaunchDate: new DateTime(2003, 6, 10),
            LandingDate: new DateTime(2004, 1, 25),
            Successful: true,
            ImagePath: "/images/mars.webp"
        ),
        new Mission(
            Name: "New Horizons",
            Description: "Space probe that conducted the first flyby of Pluto and continues to explore the Kuiper Belt.",
            LaunchDate: new DateTime(2006, 1, 19),
            LandingDate: null,
            Successful: true,
            ImagePath: "/images/neptune.webp"
        ),
        new Mission(
            Name: "Curiosity Rover",
            Description: "Large Mars rover that has been exploring Gale Crater since 2012, investigating Mars' habitability.",
            LaunchDate: new DateTime(2011, 11, 26),
            LandingDate: new DateTime(2012, 8, 6),
            Successful: true,
            ImagePath: "/images/mars.webp"
        ),
        new Mission(
            Name: "James Webb Space Telescope",
            Description: "Large infrared space telescope designed to complement and extend the discoveries of the Hubble Space Telescope.",
            LaunchDate: new DateTime(2021, 12, 25),
            LandingDate: null,
            Successful: true,
            ImagePath: "/images/neptune.webp"
        )
    }.AsReadOnly();

    /// <inheritdoc />
    public IReadOnlyList<Mission> GetAllOrderedByLaunchDate() => _missions;
}