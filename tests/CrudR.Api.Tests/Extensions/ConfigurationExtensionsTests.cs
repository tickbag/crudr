using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CrudR.Api.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;
using ConfigurationExtensions = CrudR.Api.Extensions.ConfigurationExtensions;

namespace CrudR.Api.Tests.Extensions
{
    public class ConfigurationExtensionsTests
    {
        public class TheGetValidatedMethod
        {
            [Fact]
            public void ShouldThrowArgumentNullException_WhenConfigurationIsNull()
            {
                Assert.Throws<ArgumentNullException>(() => ConfigurationExtensions.GetValidated<int>(null));
            }

            [Fact]
            public void ShouldReturnConfigurationObject_WhenConfigurationIsValid()
            {
                // Arrange
                var expectedConfig = new TestConfiguration
                {
                    TestProperty1 = "Hello",
                    TestProperty2 = "Goodbye",
                    TestProperty3 = 0,
                };

                var configuration = GetInMemoryConfigurationObject(new Dictionary<string, string>
                {
                    { "TestProperty1", "Hello" },
                    { "TestProperty2", "Goodbye" },
                    { "TestProperty3", "0" }
                });

                // Act
                var result = ConfigurationExtensions.GetValidated<TestConfiguration>(configuration);

                // Assert
                result.Should().BeEquivalentTo(expectedConfig);
            }

            [Fact]
            public void ShouldThrowConfigurationValidationException_WhenConfigurationIsInvalid()
            {
                // Arrange
                var configuration = GetInMemoryConfigurationObject(new Dictionary<string, string>
                {
                    { "TestProperty1", "Hi" },
                    { "TestProperty3", "0" }
                });

                // Act
                Func<TestConfiguration> result = () => ConfigurationExtensions.GetValidated<TestConfiguration>(configuration);

                // Act And Assert
                result.Should().Throw<ConfigurationValidationException>()
                    .WithMessage("Configuration section TestConfiguration has an invalid setting");
            }

            private IConfiguration GetInMemoryConfigurationObject(IDictionary<string, string> settings)
            {
                return new ConfigurationBuilder()
                    .AddInMemoryCollection(settings)
                    .Build();
            }

            private class TestConfiguration
            {
                [MinLength(3)]
                public string TestProperty1 { get; set; }

                [Required]
                public string TestProperty2 { get; set; }

                public int TestProperty3 { get; set; }
            }
        }
    }
}
