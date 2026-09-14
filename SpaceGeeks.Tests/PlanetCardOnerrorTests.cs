using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SpaceGeeks.Tests;

/// <summary>
/// Integration tests that verify Requirement 1.5:
/// IF any Planet_Card's image resource fails to load, THE Planet_Page SHALL remain
/// fully functional — verified by asserting every planet card img has an onerror fallback.
/// </summary>
public class PlanetCardOnerrorTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public PlanetCardOnerrorTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task IndexPage_EveryPlanetCardImg_HasNonEmptyOnerrorAttribute()
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

        // Find all <img> elements inside .planet-card elements
        var planetCardImgs = doc.DocumentNode
            .SelectNodes("//div[contains(@class,'planet-card')]//img");

        // Assert
        Assert.NotNull(planetCardImgs);
        Assert.NotEmpty(planetCardImgs);

        foreach (var img in planetCardImgs)
        {
            var onerror = img.GetAttributeValue("onerror", string.Empty);
            Assert.False(
                string.IsNullOrWhiteSpace(onerror),
                $"Expected a non-empty onerror attribute on <img> for planet card, but found: '{img.OuterHtml}'");
        }
    }
}
