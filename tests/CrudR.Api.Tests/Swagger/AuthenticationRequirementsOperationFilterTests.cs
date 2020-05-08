using System.Collections.Generic;
using CrudR.Api.Authentication;
using CrudR.Api.Swagger;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace CrudR.Api.Tests.Swagger
{
    public class AuthenticationRequirementsOperationFilterTests
    {
        public class TheApplyMethod
        {
            [Theory]
            [InlineData("GET")]
            [InlineData("Get")]
            [InlineData("get")]
            [InlineData("POST")]
            [InlineData("PUT")]
            [InlineData("DELETE")]
            public void ShouldDoNothing_WhenAuthClaimAllowsAnonymous(string httpMethod)
            {
                // Arrange
                var authClaims = new AllowAnonymoudAuthClaims();
                var operationFilter = new AuthenticationRequirementsOperationFilter(authClaims);

                var operationFilterContext = GetOperationFilterContext(httpMethod);

                var openApiOperation = new OpenApiOperation();

                // Act
                operationFilter.Apply(openApiOperation, operationFilterContext);

                // Assert
                openApiOperation.Security.Should().BeNullOrEmpty();
            }

            [Fact]
            public void ShouldAddSecurityRequirement_WhenAuthClaimDoesNotAllowAnonymous()
            {
                // Arrange
                var scheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                };
                var expectedSecurity = new OpenApiSecurityRequirement
                {
                    [scheme] = new List<string>()
                };

                var authClaims = new DisallowAnonymoudAuthClaims();
                var operationFilter = new AuthenticationRequirementsOperationFilter(authClaims);

                var operationFilterContext = GetOperationFilterContext("GET");

                var openApiOperation = new OpenApiOperation();

                // Act
                operationFilter.Apply(openApiOperation, operationFilterContext);

                // Assert
                openApiOperation.Security.Should().NotBeNullOrEmpty();
                openApiOperation.Security.Should().ContainEquivalentOf(expectedSecurity);
            }

            private OperationFilterContext GetOperationFilterContext(string httpMethod) =>
                new OperationFilterContext(
                    new ApiDescription { HttpMethod = httpMethod },
                    null,
                    null,
                    null);

            private class AllowAnonymoudAuthClaims : IAuthClaims
            {
                public bool GetAllowAnonymous => true;
                public string GetClaim => null;
                public string GetClaimValue => null;
                public bool PostAllowAnonymous => true;
                public string PostClaim => null;
                public string PostClaimValue => null;
                public bool PutAllowAnonymous => true;
                public string PutClaim => null;
                public string PutClaimValue => null;
                public bool DeleteAllowAnonymous => true;
                public string DeleteClaim => null;
                public string DeleteClaimValue => null;
            }

            private class DisallowAnonymoudAuthClaims : IAuthClaims
            {
                public bool GetAllowAnonymous => false;
                public string GetClaim => null;
                public string GetClaimValue => null;
                public bool PostAllowAnonymous => false;
                public string PostClaim => null;
                public string PostClaimValue => null;
                public bool PutAllowAnonymous => false;
                public string PutClaim => null;
                public string PutClaimValue => null;
                public bool DeleteAllowAnonymous => false;
                public string DeleteClaim => null;
                public string DeleteClaimValue => null;
            }
        }
    }
}
