using SpaceGeeks.Data;
using SpaceGeeks.Models;

namespace SpaceGeeks.Tests;

public class NasaMissionRepositoryTests
{
    [Fact]
    public void GetAllOrderedByLaunchDate_ReturnsMissionsOrderedByLaunchDate()
    {
        // Arrange
        var repository = new InMemoryNasaMissionRepository();

        // Act
        var missions = repository.GetAllOrderedByLaunchDate();

        // Assert
        Assert.NotNull(missions);
        Assert.NotEmpty(missions);
        Assert.Equal(6, missions.Count);
        
        // Verify ordering by launch date (ascending)
        for (int i = 1; i < missions.Count; i++)
        {
            Assert.True(missions[i - 1].LaunchDate <= missions[i].LaunchDate);
        }
    }

    [Fact]
    public void GetAllOrderedByLaunchDate_ReturnsImmutableList()
    {
        // Arrange
        var repository = new InMemoryNasaMissionRepository();

        // Act
        var missions = repository.GetAllOrderedByLaunchDate();
        
        // Assert
        Assert.IsAssignableFrom<IReadOnlyList<NasaMission>>(missions);
    }

    [Fact]
    public void GetAllOrderedByLaunchDate_ReturnsCorrectMissionData()
    {
        // Arrange
        var repository = new InMemoryNasaMissionRepository();

        // Act
        var missions = repository.GetAllOrderedByLaunchDate();

        // Assert
        var apolloMission = missions.FirstOrDefault(m => m.Name == "Apollo 11");
        Assert.NotNull(apolloMission);
        Assert.Equal("First crewed mission to land on the Moon", apolloMission.Description);
        Assert.Equal(new DateTime(1969, 7, 16), apolloMission.LaunchDate);
        Assert.Equal(new DateTime(1969, 7, 24), apolloMission.EndDate);
        Assert.Equal("Completed", apolloMission.Status);
        Assert.Equal("/images/apollo11.webp", apolloMission.ImagePath);
    }
}