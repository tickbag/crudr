using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrudR.Api.Exceptions;
using CrudR.Api.Filters;
using CrudR.Api.Models;
using CrudR.Api.Options;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace CrudR.Api.Tests.Filters
{
    public class RevisionActionFilterTests
    {
        public class TheOnActionExecutionAsyncMethod
        {
            [Fact]
            public async Task ShouldSetRevisionContextRequestRevision_WhenIfMatchHeaderIsSet()
            {
                // Arrange
                var ifMatchValue = Guid.NewGuid();

                var revisionContext = new RevisionContext();
                var appOptions = new ApplicationOptions();

                var revisionActionFilter = new RevisionActionFilter(revisionContext, appOptions);

                var context = GetActionExcutingContext();

                context.HttpContext.Request.Headers.Add("If-Match", ifMatchValue.ToString());

                // Act
                await revisionActionFilter.OnActionExecutionAsync(context, Next);

                // Assert
                revisionContext.RequestRevision.Should().Be(ifMatchValue);
            }

            [Theory]
            [InlineData(null)]
            [InlineData("")]
            [InlineData("  ")]
            [InlineData("blah")]
            public async Task ShouldNotSetRevisionContextRequestRevision_WhenIfMatchIsInvalid(string ifMatchValue)
            {
                // Arrange
                var revisionContext = new RevisionContext();
                var appOptions = new ApplicationOptions();

                var revisionActionFilter = new RevisionActionFilter(revisionContext, appOptions);

                var context = GetActionExcutingContext();

                if (ifMatchValue != null)
                    context.HttpContext.Request.Headers.Add("If-Match", ifMatchValue.ToString());

                // Act
                await revisionActionFilter.OnActionExecutionAsync(context, Next);

                // Assert
                revisionContext.RequestRevision.Should().BeNull();
            }

            [Theory]
            [InlineData("PUT")]
            [InlineData("DELETE")]
            public void ShouldThrowRequiredPreconditionInvalidException_WhenIfMatchIsNullRequireRevisionMatchingIsSetAndHttpMethodIsIncluded(string httpMethod)
            {
                // Arrange
                var revisionContext = new RevisionContext();
                var appOptions = new ApplicationOptions
                {
                    RequireRevisionMatching = true
                };

                var revisionActionFilter = new RevisionActionFilter(revisionContext, appOptions);

                var context = GetActionExcutingContext();
                context.HttpContext.Request.Method = httpMethod;

                // Act
                Func<Task> result = () => revisionActionFilter.OnActionExecutionAsync(context, Next);

                // Assert
                result.Should().Throw<RequiredPreconditionInvalidException>();
            }

            [Theory]
            [InlineData("GET")]
            [InlineData("POST")]
            public void ShouldNotThrowRequiredPreconditionInvalidException_WhenIfMatchIsNullRequireRevisionMatchingIsSetAndHttpMethodIsExcluded(string httpMethod)
            {
                // Arrange
                var revisionContext = new RevisionContext();
                var appOptions = new ApplicationOptions
                {
                    RequireRevisionMatching = true
                };

                var revisionActionFilter = new RevisionActionFilter(revisionContext, appOptions);

                var context = GetActionExcutingContext();
                context.HttpContext.Request.Method = httpMethod;

                // Act
                Func<Task> result = () => revisionActionFilter.OnActionExecutionAsync(context, Next);

                // Assert
                result.Should().NotThrow<RequiredPreconditionInvalidException>();
            }

            [Fact]
            public async Task ShouldSetETagHeader_WhenRevisionContextResponseRevisionIsSet()
            {
                // Arrange
                var revisionContext = new RevisionContext
                {
                    ResponseRevision = Guid.NewGuid()
                };
                var appOptions = new ApplicationOptions();

                var revisionActionFilter = new RevisionActionFilter(revisionContext, appOptions);

                var context = GetActionExcutingContext();

                // Act
                await revisionActionFilter.OnActionExecutionAsync(context, Next);

                // Assert
                context.HttpContext.Response.Headers.ContainsKey("ETag").Should().BeTrue();
                context.HttpContext.Response.Headers["ETag"].ToString().Should().Be(revisionContext.ResponseRevision.ToString());
            }

            [Fact]
            public async Task ShouldNotSetETagHeader_WhenRevisionContextResponseRevisionIsNull()
            {
                // Arrange
                var revisionContext = new RevisionContext();
                var appOptions = new ApplicationOptions();

                var revisionActionFilter = new RevisionActionFilter(revisionContext, appOptions);

                var context = GetActionExcutingContext();

                // Act
                await revisionActionFilter.OnActionExecutionAsync(context, Next);

                // Assert
                context.HttpContext.Response.Headers.ContainsKey("ETag").Should().BeFalse();
            }

            private Task<ActionExecutedContext> Next() =>
                Task.FromResult((ActionExecutedContext)null);

            private ActionExecutingContext GetActionExcutingContext() =>
                new ActionExecutingContext(
                    new ActionContext(
                        new DefaultHttpContext(),
                        new RouteData(),
                        new ActionDescriptor()),
                    new List<IFilterMetadata>(),
                    new Dictionary<string, object>(),
                    null);
        }
    }
}
