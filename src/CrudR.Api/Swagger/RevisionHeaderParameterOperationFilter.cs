using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using CrudR.Context.Abstractions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CrudR.Api.Swagger
{
    /// <summary>
    /// SwaggerGen operation filter to add the revision control header to each API endpoint description
    /// </summary>
    internal class RevisionHeaderParameterOperationFilter<T> : IOperationFilter where T : IRevisionContext
    {
        private readonly List<string> _excludeMethodsFromRevisionRequirement = new List<string>
        {
            "GET",
            "POST"
        };

        /// <inheritdoc/>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var httpMethod = context.ApiDescription.HttpMethod.ToUpperInvariant();
            if (_excludeMethodsFromRevisionRequirement.Contains(httpMethod))
                return;

            var displayAttribute = typeof(T)
                .GetProperty(nameof(IRevisionContext.RequestRevision))
                .GetCustomAttribute<DisplayAttribute>();

            if (displayAttribute == null)
                return;

            var name = displayAttribute.GetName();
            var description = displayAttribute.GetDescription();

            if (string.IsNullOrWhiteSpace(name))
                return;

            operation.Parameters ??= new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = name,
                In = ParameterLocation.Header,
                Description = description,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "String"
                }
            });
        }
    }
}
