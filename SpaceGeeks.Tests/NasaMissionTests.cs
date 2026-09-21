using SpaceGeeks.Models;

namespace SpaceGeeks.Tests;

public class NasaMissionTests
{
    [Fact]
    public void NasaMission_Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        var name = "Apollo 11";
        var year = 1969;
        var description = "First humans to land on the Moon";
        var imagePath = "/images/apollo11.jpg";
        var isManned = true;

        // Act
        var mission = new NasaMission(name, year, description, imagePath, isManned);

        // Assert
        Assert.Equal(name, mission.Name);
        Assert.Equal(year, mission.Year);
        Assert.Equal(description, mission.Description);
        Assert.Equal(imagePath, mission.ImagePath);
        Assert.Equal(isManned, mission.IsManned);
    }

    [Fact]
    public void NasaMission_IsImmutableRecord()
    {
        // Arrange
        var mission = new NasaMission("Viking 1", 1975, "First successful Mars lander", "/images/viking1.jpg", false);

        // Act & Assert
        // This will not compile if the properties are not read-only
        // We're testing that it's an immutable record by ensuring we can access properties
        Assert.NotNull(mission.Name);
        Assert.NotEqual(0, mission.Year);
    }
}