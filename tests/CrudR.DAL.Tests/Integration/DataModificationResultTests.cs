using CrudR.DAL.Integration;
using FluentAssertions;
using Xunit;

namespace CrudR.DAL.Tests.Integration
{
    public class DataModificationResultTests
    {
        public class TheConstructor
        {
            [Fact]
            public void ShouldSetRecordsModified_WhenGivenRecordsModified()
            {
                // Arrange 
                var value = 100L;

                // Act
                var result = new DataModificationResult(value);

                // Assert
                result.RecordsModified.Should().Be(value);
            }
        }
    }
}
