using Moq;
using SpaceGeeks.Data;
using SpaceGeeks.Models;
using SpaceGeeks.Pages;
using Xunit;

namespace SpaceGeeks.Tests;

public class MissionsPageModelTests
{
    [Fact]
    public void OnGet_SetsMissionsFromRepository()
    {
        // Arrange
        var missions = new List<Mission>
        {
            new Mission("Test Mission 1", "Description 1", DateTime.Now, null, true, "/images/test1.jpg"),
            new Mission("Test Mission 2", "Description 2", DateTime.Now.AddDays(-10), null, true, "/images/test2.jpg")
        };

        var mockRepo = new Mock<IMissionRepository>();
        mockRepo.Setup(r => r.GetAllOrderedByLaunchDate()).Returns(missions.AsReadOnly());

        var pageModel = new MissionsModel(mockRepo.Object);

        // Act
        pageModel.OnGet();

        // Assert
        Assert.Equal(missions, pageModel.Missions);
        mockRepo.Verify(r => r.GetAllOrderedByLaunchDate(), Times.Once);
    }

    [Fact]
    public void MissionsProperty_IsInitiallyEmpty()
    {
        // Arrange
        var mockRepo = new Mock<IMissionRepository>();
        var pageModel = new MissionsModel(mockRepo.Object);

        // Act & Assert
        Assert.Empty(pageModel.Missions);
    }

    [Fact]
    public void Constructor_StoresRepository()
    {
        // Arrange
        var mockRepo = new Mock<IMissionRepository>();

        // Act
        var pageModel = new MissionsModel(mockRepo.Object);

        // Assert
        // This test primarily verifies that the constructor works without throwing
        Assert.NotNull(pageModel);
    }
}