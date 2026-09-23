using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SpaceGeeks.Tests;

/// <summary>
/// Integration tests for the Important NASA Missions page.
/// </summary>
public class ImportantNasaMissionsPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ImportantNasaMissionsPageTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task ImportantNasaMissionsPage_LoadsSuccessfully()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/ImportantNasaMissions");
        
        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task ImportantNasaMissionsPage_HasCorrectTitle()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/ImportantNasaMissions");
        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("<title>Important NASA Missions - SpaceGeeks</title>", html);
    }

    [Fact]
    public async Task ImportantNasaMissionsPage_DisplaysImportantMissionsOnly()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/ImportantNasaMissions");
        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();

        // Parse HTML
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Find all mission cards
        var missionCards = doc.DocumentNode
            .SelectNodes("//div[contains(@class,'mission-card')]");

        // Assert
        Assert.NotNull(missionCards);
        // Based on our definition, there should be 3 important missions
        Assert.Equal(3, missionCards.Count);
    }

    [Fact]
    public async Task ImportantNasaMissionsPage_EveryMissionCardImg_HasNonEmptyOnerrorAttribute()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/ImportantNasaMissions");
        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();

        // Parse HTML
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Find all <img> elements inside .mission-card elements
        var missionCardImgs = doc.DocumentNode
            .SelectNodes("//div[contains(@class,'mission-card')]//img");

        // Assert
        Assert.NotNull(missionCardImgs);
        Assert.NotEmpty(missionCardImgs);

        foreach (var img in missionCardImgs)
        {
            var onerror = img.GetAttributeValue("onerror", string.Empty);
            Assert.False(
                string.IsNullOrWhiteSpace(onerror),
                $"Expected a non-empty onerror attribute on <img> for mission card, but found: '{img.OuterHtml}'");
        }
    }

    [Fact]
    public async Task ImportantNasaMissionsPage_DisplaysImportantMissionNames()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/ImportantNasaMissions");
        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("Apollo 11", html);
        Assert.Contains("Voyager 1", html);
        Assert.Contains("Hubble Space Telescope", html);
        // These should not appear as they're not in our important missions list
        Assert.DoesNotContain("Mars Rover Spirit", html);
    }
}