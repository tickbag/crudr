using System.Diagnostics.CodeAnalysis;
using CrudR.Core.Services;
using CrudR.Core.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace CrudR.Core
{
    /// <summary>
    /// Service Collection extension methods for registering Core services
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add core services to the Dependency injection framework
        /// </summary>
        /// <param name="services">The service collection</param>
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddTransient<IStoreService, StoreService>();
            services.AddTransient<IStoreModelValidator, StoreModelValidator>();
            services.AddTransient<IJsonArrayValidator, JsonArrayValidator>();
            services.AddTransient<IJsonObjectValidator, JsonObjectValidator>();

        }
    }
}
