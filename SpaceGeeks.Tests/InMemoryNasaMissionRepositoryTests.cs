using SpaceGeeks.Data;
using SpaceGeeks.Models;

namespace SpaceGeeks.Tests;

public class InMemoryNasaMissionRepositoryTests
{
    [Fact]
    public void GetAllOrderedByYear_ReturnsMissionsInChronologicalOrder()
    {
        // Arrange
        var repository = new InMemoryNasaMissionRepository();

        // Act
        var missions = repository.GetAllOrderedByYear();

        // Assert
        Assert.NotNull(missions);
        Assert.NotEmpty(missions);
        
        // Check that missions are ordered by year (ascending)
        for (int i = 1; i < missions.Count; i++)
        {
            Assert.True(missions[i - 1].Year <= missions[i].Year);
        }
    }

    [Fact]
    public void GetAllOrderedByYear_ReturnsReadOnlyList()
    {
        // Arrange
        var repository = new InMemoryNasaMissionRepository();

        // Act
        var missions = repository.GetAllOrderedByYear();

        // Assert
        Assert.IsAssignableFrom<IReadOnlyList<NasaMission>>(missions);
    }

    [Fact]
    public void GetAllOrderedByYear_ContainsExpectedMissions()
    {
        // Arrange
        var repository = new InMemoryNasaMissionRepository();

        // Act
        var missions = repository.GetAllOrderedByYear();

        // Assert
        Assert.Contains(missions, m => m.Name == "Apollo 11" && m.Year == 1969);
        Assert.Contains(missions, m => m.Name == "Hubble Space Telescope" && m.Year == 1990);
        Assert.Contains(missions, m => m.Name == "James Webb Space Telescope" && m.Year == 2021);
    }

    [Fact]
    public void GetAllOrderedByYear_ReturnsConsistentData()
    {
        // Arrange
        var repository = new InMemoryNasaMissionRepository();

        // Act
        var missions1 = repository.GetAllOrderedByYear();
        var missions2 = repository.GetAllOrderedByYear();

        // Assert
        Assert.Same(missions1, missions2); // Should return the same instance (readonly)
    }
}