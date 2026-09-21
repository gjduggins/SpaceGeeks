using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace SpaceGeeks.Tests;

/// <summary>
/// Integration tests for the NASA Missions page
/// </summary>
public class MissionsPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public MissionsPageTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task MissionsPage_ReturnsSuccess()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/Missions");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task MissionsPage_ReturnsCorrectContentType()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/Missions");

        // Assert
        Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
    }

    [Fact]
    public async Task MissionsPage_ContainsMissionCards()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/Missions");
        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();

        // Parse HTML
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Find all card elements
        var missionCards = doc.DocumentNode
            .SelectNodes("//div[contains(@class,'card')]");

        // Assert
        Assert.NotNull(missionCards);
        Assert.NotEmpty(missionCards);
        Assert.True(missionCards.Count >= 9, "Should contain at least 9 mission cards");
    }

    [Fact]
    public async Task MissionsPage_HasCorrectTitle()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/Missions");
        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();

        // Parse HTML
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Find title
        var titleNode = doc.DocumentNode.SelectSingleNode("//title");

        // Assert
        Assert.NotNull(titleNode);
        Assert.Equal("NASA Missions", titleNode.InnerText);
    }
}