using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CrudR.Api.Authentication;
using CrudR.Api.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CrudR.Api.Swagger
{
    internal class AuthenticationRequirementsOperationFilter : IOperationFilter
    {
        private const string AllowAnonymousProperty = "AllowAnonymous";

        private readonly IAuthClaims _authClaims;

        public AuthenticationRequirementsOperationFilter(IAuthClaims authClaims)
        {
            _authClaims = authClaims;
        }

#pragma warning disable CA1308 // Normalize strings to uppercase
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var httpMethod = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(context.ApiDescription.HttpMethod.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());

            if ((bool)typeof(IAuthClaims).GetProperty(httpMethod + AllowAnonymousProperty).GetValue(_authClaims))
                return;

            operation.Security ??= new List<OpenApiSecurityRequirement>();

            var scheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            };
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [scheme] = new List<string>()
            });
        }
#pragma warning restore CA1308 // Normalize strings to uppercase
    }
}
