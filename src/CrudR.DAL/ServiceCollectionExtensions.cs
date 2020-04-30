using CrudR.Core.Repositories;
using CrudR.DAL.Integration;
using CrudR.DAL.Mongo;
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
        public static void AddMongoDataAccessLayer(this IServiceCollection services)
        {
            services.AddScoped(typeof(IDatabaseIntegrator<>), typeof(MongoCollectionIntegrator<>));

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
