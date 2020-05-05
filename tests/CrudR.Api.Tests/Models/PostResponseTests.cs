using CrudR.Api.Models;
using FluentAssertions;
using Xunit;

namespace CrudR.Api.Tests.Models
{
    public class PostResponseTests
    {
        public class TheConstructor
        {
            [Fact]
            public void ShouldSetId_WhenGivenAnId()
            {
                // Arrange
                var uri = "test";

                // Act
                var result = new PostResponse(uri);

                // Assert
                result.Uri.Should().Be(uri);
            }
        }
    }
}
