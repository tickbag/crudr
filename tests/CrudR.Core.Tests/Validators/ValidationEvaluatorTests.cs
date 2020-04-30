using CrudR.Core.Validators;
using CrudR.Core.Validators.Models;
using FluentAssertions;
using Xunit;

namespace CrudR.Core.Tests.Validators
{
    public class ValidationEvaluatorTests
    {
        public class TheEvaluateMethod
        {
            [Fact]
            public void ShouldInvokeAndReturnValidationResultFromOnSuccessFunction_WhenEvaluationFunctionIsTrue()
            {
                // Arrange
                var expected = new ValidationResult(true);

                // Act
                var result = ValidationEvaluator.Evaluate(() => true, "", () => new ValidationResult(true));

                // Assert
                result.Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void ShouldReturnValidationResultWithNoErrorMessage_WhenEvaluationFunctionIsTrue()
            {
                // Arrange
                var expected = new ValidationResult(true);

                // Act
                var result = ValidationEvaluator.Evaluate(() => true, "Error", () => new ValidationResult(true));

                // Assert
                result.Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void ShouldReturnValidationResultOfFalseWithithGivenError_WhenEvaluationFunctionIsFalse()
            {
                // Arrange
                var expected = new ValidationResult(false, "Error");

                // Act
                var result = ValidationEvaluator.Evaluate(() => false, "Error", () => new ValidationResult(true));

                // Assert
                result.Should().BeEquivalentTo(expected);
            }
        }
    }
}
