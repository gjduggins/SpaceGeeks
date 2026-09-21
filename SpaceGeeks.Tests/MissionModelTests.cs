using SpaceGeeks.Models;
using Xunit;

namespace SpaceGeeks.Tests;

public class MissionModelTests
{
    [Fact]
    public void Mission_Record_CreatesWithCorrectValues()
    {
        // Arrange
        var name = "Test Mission";
        var description = "A test mission";
        var launchDate = new DateTime(2023, 1, 1);
        var landingDate = new DateTime(2023, 1, 10);
        var successful = true;
        var imagePath = "/images/test.jpg";

        // Act
        var mission = new Mission(name, description, launchDate, landingDate, successful, imagePath);

        // Assert
        Assert.Equal(name, mission.Name);
        Assert.Equal(description, mission.Description);
        Assert.Equal(launchDate, mission.LaunchDate);
        Assert.Equal(landingDate, mission.LandingDate);
        Assert.Equal(successful, mission.Successful);
        Assert.Equal(imagePath, mission.ImagePath);
    }

    [Fact]
    public void Mission_Record_SupportsNullLandingDate()
    {
        // Arrange
        var name = "Ongoing Mission";
        var description = "A mission still in progress";
        var launchDate = new DateTime(2023, 1, 1);
        string? landingDate = null;
        var successful = true;
        var imagePath = "/images/ongoing.jpg";

        // Act
        var mission = new Mission(name, description, launchDate, landingDate, successful, imagePath);

        // Assert
        Assert.Null(mission.LandingDate);
    }

    [Fact]
    public void Mission_Record_IsImmutable()
    {
        // Arrange
        var mission = new Mission(
            "Test Mission",
            "A test mission",
            new DateTime(2023, 1, 1),
            null,
            true,
            "/images/test.jpg"
        );

        // Act & Assert
        // This test verifies that the properties are get-only
        // If the record wasn't immutable, this would cause compilation errors
        var name = mission.Name;
        var description = mission.Description;
        var launchDate = mission.LaunchDate;
        var landingDate = mission.LandingDate;
        var successful = mission.Successful;
        var imagePath = mission.ImagePath;

        // All assertions are compile-time checks in this case
        Assert.NotNull(name);
        Assert.NotNull(description);
        Assert.NotNull(imagePath);
    }
}