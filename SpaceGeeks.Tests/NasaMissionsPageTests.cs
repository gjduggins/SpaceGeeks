using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace SpaceGeeks.Tests;

/// <summary>
/// Integration tests for the NASA Missions page
/// </summary>
public class NasaMissionsPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public NasaMissionsPageTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task NasaMissionsPage_ShouldLoadSuccessfully()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/NasaMissions");
        
        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
    }

    [Fact]
    public async Task NasaMissionsPage_ShouldContainMissionInformation()
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

        // Assert - Check that the page contains the title
        var title = doc.DocumentNode.SelectSingleNode("//h1");
        Assert.NotNull(title);
        Assert.Contains("Important NASA Missions", title.InnerText);

        // Assert - Check that the page contains mission information
        var missionItems = doc.DocumentNode.SelectNodes("//div[@class='mission-item']");
        Assert.NotNull(missionItems);
        Assert.True(missionItems.Count >= 5, "Should contain at least 5 mission items");

        // Assert - Check for specific missions
        var missionTitles = missionItems.Select(item => item.SelectSingleNode(".//h3")?.InnerText ?? "").ToList();
        Assert.Contains(missionTitles, title => title.Contains("Apollo Program"));
        Assert.Contains(missionTitles, title => title.Contains("Voyager"));
        Assert.Contains(missionTitles, title => title.Contains("Hubble Space Telescope"));
    }

    [Fact]
    public async Task NavigationMenu_ShouldContainNasaMissionsLink()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/");
        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();

        // Parse HTML
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Assert - Check that navigation contains NASA Missions link
        var navLinks = doc.DocumentNode.SelectNodes("//ul[@class='navbar-nav flex-grow-1']//a[@class='nav-link text-dark']");
        Assert.NotNull(navLinks);

        var nasaLink = navLinks.FirstOrDefault(link => link.InnerText.Contains("NASA Missions"));
        Assert.NotNull(nasaLink);
        Assert.Equal("/NasaMissions", nasaLink.GetAttributeValue("href", ""));
    }
}