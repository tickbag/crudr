using System.Collections.Generic;
using System.Text.Json;
using CrudR.Core.Validators;
using CrudR.Core.Validators.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace CrudR.Core.Tests.Validators
{
    public class JsonArrayValidatorTests
    {
        public class TheValidateMethod
        {
            // Our test data
            public static IEnumerable<object[]> TestOperands =>
                new List<object[]>
                {
                    new object[] // Two basic schema matches with an empty array
                    {
                        "[]",
                        "[]",
                        new ValidationResult(false, "Input data array is empty"),
                        0
                    },
                    new object[] // Object vs Array mismatch
                    {
                        "{}",
                        "[]",
                        new ValidationResult(false, "Input Json data definition is not an Array."),
                        0
                    },
                    new object[] // Array vs Object mismatch
                    {
                        "[]",
                        "{}",
                        new ValidationResult(false, "Stored data definition is not an Array."),
                        0
                    },
                    new object[] // Complex schema matches
                    {
                        "[ { \"myValue\": 1 }, { \"myValue\": 2 } ]",
                        "[ { \"myValue\": 1 }, { \"myValue\": 2 } ]",
                        new ValidationResult(true),
                        2
                    },
                    new object[] // Complex schema matches
                    {
                        "[ { \"myValue\": 1 }, { \"myValue\": 2 }, { \"myValue\": 3 }, { \"myValue\": 4 }, { \"myValue\": 5 } ]",
                        "[ { \"myValue\": 1 }, { \"myValue\": 2 } ]",
                        new ValidationResult(true),
                        5
                    },
                };

            [Theory]
            [MemberData(nameof(TestOperands))]
            public void ShouldReturnExpectedValidationResult_WhenGivenOperands(string inputJson, string sourceJson, object expected, int calls)
            {
                // Arrange
                using var inputJsonDoc = JsonDocument.Parse(inputJson);
                using var sourceJsonDoc = JsonDocument.Parse(sourceJson);

                var jsonObjectValidator = new Mock<IJsonObjectValidator>();
                jsonObjectValidator.Setup(validator => validator.Validate(It.IsAny<JsonElement>(), It.IsAny<JsonElement>()))
                    .Returns(new ValidationResult(true));

                var jsonArrayValidator = new JsonArrayValidator(jsonObjectValidator.Object);

                // Act
                var result = jsonArrayValidator.Validate(inputJsonDoc.RootElement, sourceJsonDoc.RootElement);

                // Assert
                result.Should().BeEquivalentTo((ValidationResult)expected);
                jsonObjectValidator.Verify(validator => validator.Validate(It.IsAny<JsonElement>(), It.IsAny<JsonElement>()), Times.Exactly(calls));
            }
        }
    }
}
