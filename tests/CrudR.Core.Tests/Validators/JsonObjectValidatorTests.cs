using System.Collections.Generic;
using System.Text.Json;
using CrudR.Core.Validators;
using CrudR.Core.Validators.Models;
using FluentAssertions;
using Xunit;

namespace CrudR.Core.Tests.Validators
{
    public class JsonObjectValidatorTests
    {
        public class TheValidateMethod
        {
            // Our test data
            public static IEnumerable<object[]> TestOperands =>
                new List<object[]>
                {
                    new object[] // Two basic schema matches
                    {
                        "{}",
                        "{}",
                        new ValidationResult(true, "")
                    },
                    new object[] // Object vs Array mismatch
                    {
                        "{}",
                        "[]",
                        new ValidationResult(false, "Stored data definition is not an Object.")
                    },
                    new object[] // Array vs Object mismatch
                    {
                        "[]",
                        "{}",
                        new ValidationResult(false, "Input Json data definition is not an Object.")
                    },
                    new object[] // Complex schema matches
                    {
                        "{ \"myValueA\": 1, \"myValueB\": \"blah\", \"myValueC\": { \"mySubValue\": 2 } }",
                        "{ \"myValueA\": 1, \"myValueB\": \"blah\", \"myValueC\": { \"mySubValue\": 2 } }",
                        new ValidationResult(true, "")
                    },
                    new object[] // Complex schema mismatch at second level
                    {
                        "{ \"myValueA\": 1, \"myValueB\": \"blah\", \"myValueC\": { \"mySubValue\": 2 } }",
                        "{ \"myValueA\": 1, \"myValueB\": \"blah\", \"myValueC\": { \"mySubValue\": \"blah2\" } }",
                        new ValidationResult(true, "")
                    },
                    new object[] // Complex schema data type mismatch
                    {
                        "{ \"myValueA\": 1, \"myValueB\": \"blah\", \"myValueC\": { \"mySubValue\": 2 } }",
                        "{ \"myValueA\": 1, \"myValueB\": 2, \"myValueC\": { \"mySubValue\": 2 } }",
                        new ValidationResult(false, "Json property myValueB has a differing data type.")
                    },
                    new object[] // Complex schema name mismatch
                    {
                        "{ \"myValueA\": 1, \"myValueB\": \"blah\", \"myValueC\": { \"mySubValue\": 2 } }",
                        "{ \"myValue\": 1, \"myValueB\": \"blah\", \"myValueC\": { \"mySubValue\": 2 } }",
                        new ValidationResult(false, "Json property 'myValueA' is missing from the stored data definition.\nJson property 'myValue' is missing from the input model.")
                    },
                    new object[] // Complex schema data type mismatch for secondary object
                    {
                        "{ \"myValueA\": 1, \"myValueB\": \"blah\", \"myValueC\": { \"mySubValue\": 2 } }",
                        "{ \"myValueA\": 1, \"myValueB\": \"blah\", \"myValueC\": [ { \"mySubValue\": 2 } ] }",
                        new ValidationResult(false, "Json property myValueC has a differing data type.")
                    },
                    new object[] // Complex schema property missing from input
                    {
                        "{ \"myValueA\": 1, \"myValueB\": \"blah\" }",
                        "{ \"myValueA\": 1, \"myValueB\": \"blah\", \"myValueC\": { \"mySubValue\": 2 } }",
                        new ValidationResult(false, "Json property 'myValueC' is missing from the input model.")
                    },
                    new object[] // Complex schema additional property on input
                    {
                        "{ \"myValueA\": 1, \"myValueB\": \"blah\", \"myValueC\": { \"mySubValue\": 2 }, \"myNewValue\": \"new\" }",
                        "{ \"myValueA\": 1, \"myValueB\": \"blah\", \"myValueC\": { \"mySubValue\": 2 } }",
                        new ValidationResult(false, "Json property 'myNewValue' is missing from the stored data definition.")
                    }
                };

            [Theory]
            [MemberData(nameof(TestOperands))]
            public void ShouldReturnExpectedValidationResult_WhenGivenOperands(string inputJson, string sourceJson, object expected)
            {
                // Arrange
                using var inputJsonDoc = JsonDocument.Parse(inputJson);
                using var sourceJsonDoc = JsonDocument.Parse(sourceJson);

                var jsonObjectValidator = new JsonObjectValidator();

                // Act
                var result = jsonObjectValidator.Validate(inputJsonDoc.RootElement, sourceJsonDoc.RootElement);

                // Assert
                result.Should().BeEquivalentTo((ValidationResult)expected);
            }
        }
    }
}
