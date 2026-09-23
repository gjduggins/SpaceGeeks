using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SpaceGeeks.Tests;

/// <summary>
/// Integration tests for the NASA Missions page.
/// </summary>
public class NasaMissionsPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public NasaMissionsPageTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task NasaMissionsPage_LoadsSuccessfully()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/NasaMissions");
        
        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task NasaMissionsPage_HasCorrectTitle()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/NasaMissions");
        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("<title>NASA Missions - SpaceGeeks</title>", html);
    }

    [Fact]
    public async Task NasaMissionsPage_DisplaysAllMissions()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/NasaMissions");
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
        Assert.Equal(5, missionCards.Count);
    }

    [Fact]
    public async Task NasaMissionsPage_EveryMissionCardImg_HasNonEmptyOnerrorAttribute()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/NasaMissions");
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
    public async Task NasaMissionsPage_DisplaysMissionNames()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/NasaMissions");
        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("Apollo 11", html);
        Assert.Contains("Voyager 1", html);
        Assert.Contains("Hubble Space Telescope", html);
        Assert.Contains("Mars Rover Perseverance", html);
        Assert.Contains("James Webb Space Telescope", html);
    }
}