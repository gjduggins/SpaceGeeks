using Moq;
using SpaceGeeks.Data;
using SpaceGeeks.Models;
using SpaceGeeks.Pages;

namespace SpaceGeeks.Tests;

public class NasaMissionsPageModelTests
{
    [Fact]
    public void OnGet_WithNoFilter_ReturnsAllMissions()
    {
        // Arrange
        var missions = new List<NasaMission>
        {
            new NasaMission("Mission 1", "Description 1", DateTime.Now, null, "Active", "/image1.jpg"),
            new NasaMission("Mission 2", "Description 2", DateTime.Now, null, "Completed", "/image2.jpg")
        };
        
        var mockRepo = new Mock<INasaMissionRepository>();
        mockRepo.Setup(r => r.GetAllOrderedByLaunchDate()).Returns(missions);
        
        var pageModel = new NasaMissionsModel(mockRepo.Object);
        pageModel.StatusFilter = null;

        // Act
        pageModel.OnGet();

        // Assert
        Assert.Equal(2, pageModel.Missions.Count);
        Assert.Contains(pageModel.Missions, m => m.Name == "Mission 1");
        Assert.Contains(pageModel.Missions, m => m.Name == "Mission 2");
    }

    [Fact]
    public void OnGet_WithActiveStatusFilter_ReturnsOnlyActiveMissions()
    {
        // Arrange
        var missions = new List<NasaMission>
        {
            new NasaMission("Active Mission", "Description 1", DateTime.Now, null, "Active", "/image1.jpg"),
            new NasaMission("Completed Mission", "Description 2", DateTime.Now, null, "Completed", "/image2.jpg")
        };
        
        var mockRepo = new Mock<INasaMissionRepository>();
        mockRepo.Setup(r => r.GetAllOrderedByLaunchDate()).Returns(missions);
        
        var pageModel = new NasaMissionsModel(mockRepo.Object);
        pageModel.StatusFilter = "Active";

        // Act
        pageModel.OnGet();

        // Assert
        Assert.Single(pageModel.Missions);
        Assert.Equal("Active Mission", pageModel.Missions[0].Name);
        Assert.Equal("Active", pageModel.Missions[0].Status);
    }

    [Fact]
    public void OnGet_WithCompletedStatusFilter_ReturnsOnlyCompletedMissions()
    {
        // Arrange
        var missions = new List<NasaMission>
        {
            new NasaMission("Active Mission", "Description 1", DateTime.Now, null, "Active", "/image1.jpg"),
            new NasaMission("Completed Mission", "Description 2", DateTime.Now, null, "Completed", "/image2.jpg"),
            new NasaMission("Another Completed Mission", "Description 3", DateTime.Now, null, "Completed", "/image3.jpg")
        };
        
        var mockRepo = new Mock<INasaMissionRepository>();
        mockRepo.Setup(r => r.GetAllOrderedByLaunchDate()).Returns(missions);
        
        var pageModel = new NasaMissionsModel(mockRepo.Object);
        pageModel.StatusFilter = "Completed";

        // Act
        pageModel.OnGet();

        // Assert
        Assert.Equal(2, pageModel.Missions.Count);
        Assert.All(pageModel.Missions, m => Assert.Equal("Completed", m.Status));
    }

    [Fact]
    public void OnGet_WithInvalidStatusFilter_ReturnsNoMissions()
    {
        // Arrange
        var missions = new List<NasaMission>
        {
            new NasaMission("Active Mission", "Description 1", DateTime.Now, null, "Active", "/image1.jpg"),
            new NasaMission("Completed Mission", "Description 2", DateTime.Now, null, "Completed", "/image2.jpg")
        };
        
        var mockRepo = new Mock<INasaMissionRepository>();
        mockRepo.Setup(r => r.GetAllOrderedByLaunchDate()).Returns(missions);
        
        var pageModel = new NasaMissionsModel(mockRepo.Object);
        pageModel.StatusFilter = "InvalidStatus";

        // Act
        pageModel.OnGet();

        // Assert
        Assert.Empty(pageModel.Missions);
    }

    [Fact]
    public void OnGet_WithCaseInsensitiveStatusFilter_ReturnsMatchingMissions()
    {
        // Arrange
        var missions = new List<NasaMission>
        {
            new NasaMission("Active Mission", "Description 1", DateTime.Now, null, "Active", "/image1.jpg"),
            new NasaMission("Completed Mission", "Description 2", DateTime.Now, null, "Completed", "/image2.jpg")
        };
        
        var mockRepo = new Mock<INasaMissionRepository>();
        mockRepo.Setup(r => r.GetAllOrderedByLaunchDate()).Returns(missions);
        
        var pageModel = new NasaMissionsModel(mockRepo.Object);
        pageModel.StatusFilter = "active"; // lowercase

        // Act
        pageModel.OnGet();

        // Assert
        Assert.Single(pageModel.Missions);
        Assert.Equal("Active Mission", pageModel.Missions[0].Name);
    }
}