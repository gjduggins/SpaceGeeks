using SpaceGeeks.Models;

namespace SpaceGeeks.Data;

/// <summary>
/// In-memory implementation of <see cref="INasaMissionRepository"/>.
/// NASA mission data is stored as a static, hardcoded list initialised once at class load time,
/// ordered by ascending year.
/// </summary>
public sealed class InMemoryNasaMissionRepository : INasaMissionRepository
{
    private static readonly IReadOnlyList<NasaMission> _missions = new List<NasaMission>
    {
        new NasaMission("Explorer 1", 1958, "America's first successful satellite, discovered the Van Allen radiation belts.", "/images/explorer1.jpg", false),
        new NasaMission("Mercury-Redstone 3", 1961, "First American human spaceflight piloted by Alan Shepard.", "/images/mercury-redstone3.jpg", true),
        new NasaMission("Gemini 3", 1965, "First crewed Gemini flight with John Young and Gus Grissom.", "/images/gemini3.jpg", true),
        new NasaMission("Apollo 7", 1968, "First crewed Apollo mission to test the Command and Service Module.", "/images/apollo7.jpg", true),
        new NasaMission("Apollo 8", 1968, "First crewed spacecraft to orbit the Moon.", "/images/apollo8.jpg", true),
        new NasaMission("Apollo 11", 1969, "First humans to land on the Moon: Neil Armstrong and Buzz Aldrin.", "/images/apollo11.jpg", true),
        new NasaMission("Apollo 17", 1972, "Last Apollo mission to the Moon, first geologist on the lunar surface.", "/images/apollo17.jpg", true),
        new NasaMission("Skylab 1", 1973, "America's first space station.", "/images/skylab1.jpg", true),
        new NasaMission("Viking 1", 1975, "First successful Mars lander mission.", "/images/viking1.jpg", false),
        new NasaMission("Space Shuttle Columbia", 1981, "First orbital flight of the Space Shuttle program.", "/images/shuttle-columbia.jpg", true),
        new NasaMission("Hubble Space Telescope", 1990, "Revolutionary space telescope that transformed astronomy.", "/images/hubble.jpg", false),
        new NasaMission("Mars Pathfinder", 1996, "Demonstrated innovative airbag landing technology on Mars.", "/images/pathfinder.jpg", false),
        new NasaMission("International Space Station", 1998, "Ongoing multinational orbital laboratory.", "/images/iss.jpg", true),
        new NasaMission("Spirit and Opportunity Rovers", 2003, "Twin Mars rovers that far exceeded their mission lifespans.", "/images/spirit-opportunity.jpg", false),
        new NasaMission("New Horizons", 2006, "Flyby mission to Pluto and the Kuiper Belt object Arrokoth.", "/images/newhorizons.jpg", false),
        new NasaMission("Kepler Space Telescope", 2009, "Discovered thousands of exoplanets in our galaxy.", "/images/kepler.jpg", false),
        new NasaMission("Curiosity Rover", 2011, "Large rover exploring Gale Crater on Mars.", "/images/curiosity.jpg", false),
        new NasaMission("Perseverance Rover", 2020, "Currently exploring Jezero Crater on Mars, collecting samples for future return to Earth.", "/images/perseverance.jpg", false),
        new NasaMission("James Webb Space Telescope", 2021, "Successor to Hubble, observing the universe in infrared.", "/images/webb.jpg", false),
        new NasaMission("Artemis Program", 2022, "Current program aiming to return humans to the Moon and establish a sustainable presence.", "/images/artemis.jpg", true)
    }.AsReadOnly();

    /// <inheritdoc />
    public IReadOnlyList<NasaMission> GetAllOrderedByYear() => _missions;
}