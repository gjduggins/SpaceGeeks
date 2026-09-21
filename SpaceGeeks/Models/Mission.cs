using System.ComponentModel.DataAnnotations;

namespace SpaceGeeks.Models;

/// <summary>
/// Represents an important NASA space mission with its key facts.
/// This is an immutable record — all properties are set at construction time.
/// </summary>
public sealed record Mission(
    [property: Required] string Name,
    [property: Required] string Description,
    DateTime LaunchDate,
    string? LandingDate,
    bool Successful,
    string ImagePath          // relative URL to static asset, e.g. "/images/apollo11.jpg"
);