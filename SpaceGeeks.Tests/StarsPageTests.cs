using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SpaceGeeks.Tests;

public class StarsPageTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public StarsPageTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task StarsPage_LoadsSuccessfully()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/Stars");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task StarsPage_HasCorrectTitle()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/Stars");
        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Contains("<title>Stars - SpaceGeeks</title>", html);
    }

    [Fact]
    public async Task StarsPage_DisplaysAllStars()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/Stars");
        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();

        // Parse HTML
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Find all star cards
        var starCards = doc.DocumentNode
          .SelectNodes("//div[contains(@class,'star-card')]");

        // Assert
        Assert.NotNull(starCards);
        Assert.Equal(5, starCards.Count); // Assuming there are 5 stars for this test
    }
}