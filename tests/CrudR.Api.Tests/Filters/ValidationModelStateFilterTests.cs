using System.Collections.Generic;
using System.Threading.Tasks;
using CrudR.Api.Filters;
using CrudR.Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CrudR.Api.Tests.Filters
{
    public class ValidationModelStateFilterTests
    {
        public class TheOnActionExecutionAsyncMethod
        {
            [Fact]
            public async Task ShouldReturnBadRequest_WhenStateIsInvalid()
            {
                // Arrange
                var expectedErrorWrapper = new ErrorResponse("ValidationError FakeError: FakeMessage");
                var loggerMock = new Mock<ILogger<ValidateModelStateFilter>>();
                var filter = new ValidateModelStateFilter(loggerMock.Object);
                var context = GetActionExecutingContext();
                context.ModelState.AddModelError("FakeError", "FakeMessage");

                // Act
                await filter.OnActionExecutionAsync(context, null);

                // Assert
                context.Result.Should().BeOfType<BadRequestObjectResult>()
                    .Which.Value.Should().BeEquivalentTo(expectedErrorWrapper);
            }

            [Fact]
            public async Task ShouldCallNext_WhenStateIsValid()
            {
                // Arrange
                var loggerMock = new Mock<ILogger<ValidateModelStateFilter>>();
                var filter = new ValidateModelStateFilter(loggerMock.Object);
                var context = GetActionExecutingContext();
                var isCalled = false;

                Task<ActionExecutedContext> Next()
                {
                    isCalled = true;
                    return Task.FromResult((ActionExecutedContext)null);
                }

                // Act
                await filter.OnActionExecutionAsync(context, Next);

                // Assert
                isCalled.Should().BeTrue();
            }

            private ActionExecutingContext GetActionExecutingContext() =>
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
