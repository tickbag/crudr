using System;
using CrudR.Core.Repositories;
using CrudR.DAL.Integration;
using CrudR.DAL.Mongo;
using CrudR.DAL.Options;
using CrudR.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CrudR.DAL
{
    /// <summary>
    /// Service Collection extension methods for registering Data Access Layer services
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add and use a Mongo database
        /// </summary>
        /// <param name="services">The services collection</param>
        /// <param name="databaseOptions">The database configuration options</param>
        public static void AddMongoDataAccessLayer(this IServiceCollection services, IDatabaseOptions databaseOptions) =>
            AddMongoDataAccessLayer(services, databaseOptions, null);

        /// <summary>
        /// Add and use a Mongo database
        /// </summary>
        /// <param name="services">The services collection</param>
        /// <param name="databaseOptions">The database configuration options</param>
        /// <param name="healthCheckBuilder">An instance of the health check builder</param>
        /// <param name="healthCheckTag">The tag to register the MongoDb health check against</param>
        public static void AddMongoDataAccessLayer(this IServiceCollection services, IDatabaseOptions databaseOptions, IHealthChecksBuilder healthCheckBuilder = null, string healthCheckTag = null)
        {
            _ = databaseOptions ?? throw new ArgumentNullException(nameof(databaseOptions));

            services.AddScoped(typeof(IDatabaseIntegrator<>), typeof(MongoCollectionIntegrator<>));

            if (healthCheckBuilder != null)
                healthCheckBuilder.AddMongoDb(databaseOptions.ConnectionString, databaseOptions.DatabaseName, null, new[] { healthCheckTag });

            services.AddDataAccessLayer();
        }

        /// <summary>
        /// Add the generic data access layer services
        /// </summary>
        /// <param name="services">The services collection</param>
        public static void AddDataAccessLayer(this IServiceCollection services)
        {
            services.AddTransient<IStoreRepository, StoreRepository>();
        }
    }
}
