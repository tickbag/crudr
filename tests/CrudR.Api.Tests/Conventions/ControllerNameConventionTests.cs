using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using CrudR.Api.Conventions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Xunit;

namespace CrudR.Api.Tests.Conventions
{
    public class ControllerNameConventionTests
    {
        public class TheApplyMethod
        {
            [Fact]
            public void ShouldThrowArgumentNullException_WhenControllerModelIsNull()
            {
                // Arrange
                var newName = "test";
                var convention = new ControllerNameConvention(newName);

                // Act
                Action result = () => convention.Apply(null);

                // Assert
                result.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("controller");
            }

            [Theory]
            [InlineData(null, "Test")]
            [InlineData("", "")]
            [InlineData("blah", "blah")]
            public void ShouldSetControllerNameAsExpected_WhenProvidedWithNewName(string newName, string expected)
            {
                // Arrange
                var convention = new ControllerNameConvention(newName);

                var controllerAttributes = new List<object>();
                var controllerModel = new ControllerModel(typeof(TestController).GetTypeInfo(), controllerAttributes);
                controllerModel.ControllerName = "Test";

                var expectedControllerModel = new ControllerModel(typeof(TestController).GetTypeInfo(), controllerAttributes);
                expectedControllerModel.ControllerName = expected;

                // Act
                convention.Apply(controllerModel);

                // Assert
                controllerModel.Should().BeEquivalentTo(expectedControllerModel);
            }

            private class TestController : ControllerBase
            { }
        }
    }
}
