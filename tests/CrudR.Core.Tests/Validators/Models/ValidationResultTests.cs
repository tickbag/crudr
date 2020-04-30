using System.Collections.Generic;
using FluentAssertions;
using CrudR.Core.Validators.Models;
using Xunit;

namespace CrudR.Core.Tests.Validators.Models
{
    public class ValidationResultTests
    {
        public class TheConstructor
        {
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void ShouldSetErrorsToNull_WhenGivenIsValidOnly(bool isValid)
            {
                // Arrange / Act
                var result = new ValidationResult(isValid);

                // Assert
                result.Errors.Should().BeNull();
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void ShouldSetIsValid_WhenGivenIsValidOnly(bool isValid)
            {
                // Arrange / Act
                var result = new ValidationResult(isValid);

                // Assert
                result.IsValid.Should().Be(isValid);
            }

            [Theory]
            [InlineData(true, "Error 1")]
            [InlineData(false, "Error 2")]
            public void ShouldSetIsValidAndErrors_WhenGivenIsValidAndErrors(bool isValid, string errors)
            {
                // Arrange / Act
                var result = new ValidationResult(isValid, errors);

                // Assert
                result.IsValid.Should().Be(isValid);
                result.Errors.Should().Be(errors);
            }
        }

        public class ThePlusOperator
        {
            // Our test data
            public static IEnumerable<object[]> TestOperands =>
                new List<object[]>
                {
                    new object[]
                    {
                        new ValidationResult(true),
                        new ValidationResult(true),
                        new ValidationResult(true, "")
                    },
                    new object[]
                    {
                        new ValidationResult(true),
                        new ValidationResult(false, "Error on operand B"),
                        new ValidationResult(false, "Error on operand B")
                    },
                    new object[]
                    {
                        new ValidationResult(false, "Error on operand A"),
                        new ValidationResult(true),
                        new ValidationResult(false, "Error on operand A")
                    },
                    new object[]
                    {
                        new ValidationResult(false, "Error on operand A"),
                        new ValidationResult(false, "Error on operand B"),
                        new ValidationResult(false, "Error on operand A\nError on operand B")
                    },
                    new object[]
                    {
                        null,
                        new ValidationResult(true),
                        new ValidationResult(true, "")
                    },
                    new object[]
                    {
                        new ValidationResult(true),
                        null,
                        new ValidationResult(true, "")
                    },
                    new object[]
                    {
                        null,
                        new ValidationResult(false, "Error on operand B"),
                        new ValidationResult(false, "Error on operand B")
                    },
                    new object[]
                    {
                        new ValidationResult(false, "Error on operand A"),
                        null,
                        new ValidationResult(false, "Error on operand A")
                    },
                    new object[]
                    {
                        null,
                        null,
                        null
                    },
                };

            [Theory]
            [MemberData(nameof(TestOperands))]
            public void ShouldReturnExpectedResult_WhenGivenOperands(object opA, object opB, object expected)
            {
                // Act
                var result = (ValidationResult)opA + (ValidationResult)opB;

                // Assert
                result.Should().BeEquivalentTo((ValidationResult)expected);
            }
        }
    }
}
