using SpaceGeeks.Data;
using SpaceGeeks.Models;
using Xunit;

namespace SpaceGeeks.Tests;

public class MissionRepositoryTests
{
    [Fact]
    public void GetAllOrderedByLaunchDate_ReturnsMissionsInChronologicalOrder()
    {
        // Arrange
        var repository = new InMemoryMissionRepository();

        // Act
        var missions = repository.GetAllOrderedByLaunchDate();

        // Assert
        Assert.NotNull(missions);
        Assert.NotEmpty(missions);
        
        // Verify missions are ordered by launch date (ascending)
        for (int i = 1; i < missions.Count; i++)
        {
            Assert.True(missions[i - 1].LaunchDate <= missions[i].LaunchDate);
        }
    }

    [Fact]
    public void GetAllOrderedByLaunchDate_ReturnsImmutableList()
    {
        // Arrange
        var repository = new InMemoryMissionRepository();

        // Act
        var missions = repository.GetAllOrderedByLaunchDate();
        
        // Assert
        Assert.True(missions is IReadOnlyList<Mission>);
    }

    [Fact]
    public void GetAllOrderedByLaunchDate_ContainsExpectedMissions()
    {
        // Arrange
        var repository = new InMemoryMissionRepository();

        // Act
        var missions = repository.GetAllOrderedByLaunchDate();

        // Assert
        Assert.Contains(missions, m => m.Name == "Apollo 11");
        Assert.Contains(missions, m => m.Name == "Voyager 1");
        Assert.Contains(missions, m => m.Name == "Hubble Space Telescope");
        Assert.Contains(missions, m => m.Name == "Mars Pathfinder");
        Assert.Contains(missions, m => m.Name == "International Space Station");
        Assert.Contains(missions, m => m.Name == "Spirit and Opportunity Rovers");
        Assert.Contains(missions, m => m.Name == "New Horizons");
        Assert.Contains(missions, m => m.Name == "Curiosity Rover");
        Assert.Contains(missions, m => m.Name == "James Webb Space Telescope");
    }
}