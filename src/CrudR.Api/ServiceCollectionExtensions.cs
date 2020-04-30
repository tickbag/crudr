using System.Diagnostics.CodeAnalysis;
using CrudR.Api.Models;
using CrudR.Context.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace CrudR.Api
{
    /// <summary>
    /// Service Collection extensions to add Api services
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Service Collection extension method to add Api services
        /// </summary>
        /// <param name="services">The service collection</param>
        public static void AddApiServices(this IServiceCollection services)
        {
            services.AddScoped<IRevisionContext, RevisionContext>();

        }
    }
}
