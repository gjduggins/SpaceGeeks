namespace SpaceGeeks.Models;

/// <summary>
/// Represents a NASA space mission with its key facts.
/// This is an immutable record — all properties are set at construction time.
/// </summary>
public sealed record NasaMission(
    string Name,
    string Description,
    DateTime LaunchDate,
    DateTime? EndDate,
    string Status,           // Active, Completed, Failed
    string ImagePath         // relative URL to static asset, e.g. "/images/apollo11.webp"
);