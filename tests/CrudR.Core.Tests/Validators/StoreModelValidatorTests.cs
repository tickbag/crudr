using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using CrudR.Core.Models;
using CrudR.Core.Tests.Helpers;
using CrudR.Core.Validators;
using FluentAssertions;
using Moq;
using Xunit;
using ValidationResult = CrudR.Core.Validators.Models.ValidationResult;

namespace CrudR.Core.Tests.Validators
{
    public class StoreModelValidatorTests
    {
        public class TheValidateMethod
        {
            // Our test data
            public static IEnumerable<object[]> TestOperands =>
                new List<object[]>
                {
                    new object[]
                    {
                        null,
                        "{}",
                        null,
                        null,
                        new ExceptionExpected<ArgumentNullException>(assertion => assertion.And.ParamName.Should().Be("input"))
                    },
                    new object[]
                    {
                        "{}",
                        null,
                        null,
                        null,
                        new ExceptionExpected<ArgumentNullException>(assertion => assertion.And.ParamName.Should().Be("source"))
                    },
                    new object[]
                    {
                        "[]",
                        "[]",
                        new ValidationResult(true),
                        new ValidationResult(false),
                        new ExceptionNotExpected()
                    },
                    new object[]
                    {
                        "{}",
                        "{}",
                        new ValidationResult(false),
                        new ValidationResult(true),
                        new ExceptionNotExpected()
                    },
                    new object[]
                    {
                        "{}",
                        "[]",
                        new ValidationResult(false, "Array Error"),
                        new ValidationResult(false, "Object Error"),
                        new ExceptionExpected<ValidationException>(assertion => assertion.WithMessage("Object Error"))
                    }
                };

            [Theory]
            [MemberData(nameof(TestOperands))]
            public void ShouldReturnExpectedResult_WhenGivenInputAndSourceModels(
                string inputJson,
                string sourceJson,
                object arrayValidatorReturns,
                object objectValidatorReturns,
                IExceptionAssertion expected)
            {
                // Arrange
                using var inputJsonDoc = JsonDocument.Parse(inputJson ?? "{}");
                using var sourceJsonDoc = JsonDocument.Parse(sourceJson ?? "{}");

                var inputStoreModel = string.IsNullOrEmpty(inputJson) ? null : new StoreModel("/input", inputJsonDoc.RootElement);
                var sourceStoreModel = string.IsNullOrEmpty(sourceJson) ? null : new StoreModel("/source", sourceJsonDoc.RootElement);

                var jsonArrayValidator = GetJsonArrayValidatorMock((ValidationResult)arrayValidatorReturns);
                var jsonObjectValidator = GetJsonObjectValidatorMock((ValidationResult)objectValidatorReturns);

                var storeValidationService = new StoreModelValidator(jsonArrayValidator.Object, jsonObjectValidator.Object);

                // Act
                Action result = () => storeValidationService.Validate(inputStoreModel, sourceStoreModel);

                // Assert
                expected.Assert(result);
            }

            private static Mock<IJsonArrayValidator> GetJsonArrayValidatorMock(ValidationResult returns)
            {
                var jsonArrayValidator = new Mock<IJsonArrayValidator>();
                jsonArrayValidator.Setup(validator => validator.Validate(It.IsAny<JsonElement>(), It.IsAny<JsonElement>()))
                    .Returns(returns);

                return jsonArrayValidator;
            }

            private static Mock<IJsonObjectValidator> GetJsonObjectValidatorMock(ValidationResult returns)
            {
                var jsonObjectValidator = new Mock<IJsonObjectValidator>();
                jsonObjectValidator.Setup(validator => validator.Validate(It.IsAny<JsonElement>(), It.IsAny<JsonElement>()))
                        .Returns(returns);

                return jsonObjectValidator;
            }
        }
    }
}
