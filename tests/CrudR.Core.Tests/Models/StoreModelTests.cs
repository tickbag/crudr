using System.Text.Json;
using CrudR.Core.Models;
using FluentAssertions;
using Xunit;

namespace CrudR.Core.Tests.Models
{
    public class StoreModelTests
    {
        public class TheConstructor
        {
            [Fact]
            public void ShouldSetId_WhenGivenAnId()
            {
                // Arrange
                var id = "test";

                // Act
                var result = new StoreModel(id, new JsonElement());

                // Assert
                result.Id.Should().Be(id);
            }

            [Fact]
            public void ShouldSetPayload_GivenAPayload()
            {
                // Arrange
                var payload = new JsonElement();

                // Act
                var result = new StoreModel("", payload);

                // Assert
                result.Payload.Should().Be(payload);
            }
        }
    }
}
