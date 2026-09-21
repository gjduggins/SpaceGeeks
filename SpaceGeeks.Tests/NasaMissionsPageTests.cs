using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SpaceGeeks.Tests;

/// <summary>
/// Integration tests for the NASA Missions timeline page.
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
        Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
    }

    [Fact]
    public async Task NasaMissionsPage_ContainsTimelineTitle()
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

        // Assert
        var titleElement = doc.DocumentNode.SelectSingleNode("//h1");
        Assert.NotNull(titleElement);
        Assert.Contains("NASA Missions Timeline", titleElement.InnerText);
    }

    [Fact]
    public async Task NasaMissionsPage_ContainsMissionItems()
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

        // Find timeline items
        var timelineItems = doc.DocumentNode
            .SelectNodes("//div[contains(@class,'timeline-item')]");

        // Assert
        Assert.NotNull(timelineItems);
        Assert.NotEmpty(timelineItems);
        Assert.True(timelineItems.Count >= 10, "Should contain at least 10 mission items");
    }

    [Fact]
    public async Task NasaMissionsPage_MissionItemsContainRequiredElements()
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

        // Find the first timeline item
        var firstTimelineItem = doc.DocumentNode
            .SelectSingleNode("//div[contains(@class,'timeline-content')]");

        // Assert
        Assert.NotNull(firstTimelineItem);
        
        // Check for required elements
        var yearElement = firstTimelineItem.SelectSingleNode(".//div[contains(@class,'mission-year')]");
        var nameElement = firstTimelineItem.SelectSingleNode(".//h3");
        var descriptionElement = firstTimelineItem.SelectSingleNode(".//p[contains(@class,'mission-description')]");
        
        Assert.NotNull(yearElement);
        Assert.NotNull(nameElement);
        Assert.NotNull(descriptionElement);
    }

    [Fact]
    public async Task NasaMissionsPage_NavigationLinkExists()
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

        // Find NASA Missions navigation link
        var navLinks = doc.DocumentNode
            .SelectNodes("//a[@class='nav-link']");

        // Assert
        Assert.NotNull(navLinks);
        Assert.Contains(navLinks, link => link.GetAttributeValue("href", "") == "/NasaMissions");
    }
}