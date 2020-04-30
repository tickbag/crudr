using System;
using System.ComponentModel.DataAnnotations;
using CrudR.Api.Swagger;
using CrudR.Context.Abstractions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace CrudR.Api.Tests.Swagger
{
    public class RevisionHeaderParameterOperationFilterTests
    {
        public class TheApplyMethod
        {
            [Theory]
            [InlineData("GET")]
            [InlineData("POST")]
            public void ShouldDoNothing_WhenHttpMethodIsExcluded(string httpMethod)
            {
                // Arrange
                var operationFilter = new RevisionHeaderParameterOperationFilter<RevisionContext>();

                var operationFilterContext = GetOperationFilterContext(httpMethod);

                var openApiOperation = new OpenApiOperation();

                // Act
                operationFilter.Apply(openApiOperation, operationFilterContext);

                // Assert
                openApiOperation.Parameters.Should().BeNullOrEmpty();
            }

            [Fact]
            public void ShouldDoNothing_WhenDisplayAttributeIsNotSet()
            {
                // Arrange
                var operationFilter = new RevisionHeaderParameterOperationFilter<RevisionContextNoDisplay>();

                var operationFilterContext = GetOperationFilterContext("PUT");

                var openApiOperation = new OpenApiOperation();

                // Act
                operationFilter.Apply(openApiOperation, operationFilterContext);

                // Assert
                openApiOperation.Parameters.Should().BeNullOrEmpty();
            }

            [Fact]
            public void ShouldDoNothing_WhenDisplayAttributeIsSetAndNameIsNull()
            {
                // Arrange
                var operationFilter = new RevisionHeaderParameterOperationFilter<RevisionContextNoDipslayName>();

                var operationFilterContext = GetOperationFilterContext("PUT");

                var openApiOperation = new OpenApiOperation();

                // Act
                operationFilter.Apply(openApiOperation, operationFilterContext);

                // Assert
                openApiOperation.Parameters.Should().BeNullOrEmpty();
            }

            [Fact]
            public void ShouldDoNothing_WhenDisplayAttributeIsSetAndNameIsEmpty()
            {
                // Arrange
                var operationFilter = new RevisionHeaderParameterOperationFilter<RevisionContextEmptyDipslayName>();

                var operationFilterContext = GetOperationFilterContext("PUT");

                var openApiOperation = new OpenApiOperation();

                // Act
                operationFilter.Apply(openApiOperation, operationFilterContext);

                // Assert
                openApiOperation.Parameters.Should().BeNullOrEmpty();
            }

            [Fact]
            public void ShouldDoNothing_WhenDisplayAttributeIsSetAndNameIsWhitespace()
            {
                // Arrange
                var operationFilter = new RevisionHeaderParameterOperationFilter<RevisionContextWhiteSpaceDipslayName>();

                var operationFilterContext = GetOperationFilterContext("PUT");

                var openApiOperation = new OpenApiOperation();

                // Act
                operationFilter.Apply(openApiOperation, operationFilterContext);

                // Assert
                openApiOperation.Parameters.Should().BeNullOrEmpty();
            }

            [Theory]
            [InlineData("PUT")]
            [InlineData("DELETE")]
            public void ShouldAddParameter_WhenDisplayAttributeIsSetAndHttpMethodIsIncluded(string httpMethod)
            {
                // Arrange
                var expectedParameter = new OpenApiParameter
                {
                    Name = "If-Match",
                    In = ParameterLocation.Header,
                    Description = "Blah",
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "String"
                    }
                };

                var operationFilter = new RevisionHeaderParameterOperationFilter<RevisionContext>();

                var operationFilterContext = GetOperationFilterContext(httpMethod);

                var openApiOperation = new OpenApiOperation();

                // Act
                operationFilter.Apply(openApiOperation, operationFilterContext);

                // Assert
                openApiOperation.Parameters.Should().NotBeNullOrEmpty();
                openApiOperation.Parameters.Should().ContainEquivalentOf(expectedParameter);
            }

            private class RevisionContext : IRevisionContext
            {
                [Display(Name = "If-Match", Description = "Blah")]
                public Guid? RequestRevision { get; set; }
                public Guid? ResponseRevision { get; set; }
            }

            private class RevisionContextNoDisplay : IRevisionContext
            {
                public Guid? RequestRevision { get; set; }
                public Guid? ResponseRevision { get; set; }
            }

            private class RevisionContextNoDipslayName : IRevisionContext
            {
                [Display(Description = "Blah")]
                public Guid? RequestRevision { get; set; }
                public Guid? ResponseRevision { get; set; }
            }

            private class RevisionContextEmptyDipslayName : IRevisionContext
            {
                [Display(Name = "", Description = "Blah")]
                public Guid? RequestRevision { get; set; }
                public Guid? ResponseRevision { get; set; }
            }

            private class RevisionContextWhiteSpaceDipslayName : IRevisionContext
            {
                [Display(Name = "", Description = "Blah")]
                public Guid? RequestRevision { get; set; }
                public Guid? ResponseRevision { get; set; }
            }

            private OperationFilterContext GetOperationFilterContext(string httpMethod) =>
                new OperationFilterContext(
                    new ApiDescription { HttpMethod = httpMethod },
                    null,
                    null,
                    null);
        }
    }
}
