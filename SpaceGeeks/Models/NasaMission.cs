namespace SpaceGeeks.Models;

/// <summary>
/// Represents a NASA mission with key facts and timeline information.
/// This is an immutable record — all properties are set at construction time.
/// </summary>
public sealed record NasaMission(
    string Name,
    int Year,
    string Description,
    string ImagePath,          // relative URL to static asset, e.g. "/images/apollo11.jpg"
    bool IsManned
);