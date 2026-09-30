using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace SpaceGeeks.Tests
{
    public class StarsPageTests
    {
        [Fact]
        public void StarsPage_ReturnsCorrectView()
        {
            // Arrange
            var viewResult = new ViewResult();

            // Act
            var result = viewResult.ViewName;

            // Assert
            Assert.Equal("Stars", result);
        }
    }
}