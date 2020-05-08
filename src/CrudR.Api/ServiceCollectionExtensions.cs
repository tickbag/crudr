using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using CrudR.Api.ExceptionResponseHandlers;
using CrudR.Api.Exceptions;
using CrudR.Api.Models;
using CrudR.Context.Abstractions;
using CrudR.Core.Exceptions;
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

            // Add Exception Response Handlers
            services.AddTransient<IApiExceptionResponseHandler<ValidationException>, ValidationExceptionResponseHandler>();
            services.AddTransient<IApiExceptionResponseHandler<RecordNotFoundException>, RecordNotFoundExceptionResponseHandler>();
            services.AddTransient<IApiExceptionResponseHandler<RecordNotModifiedException>, RecordNotModifiedExceptionResponseHandler>();
            services.AddTransient<IApiExceptionResponseHandler<RecordAlreadyExistsException>, RecordAlreadyExistsExceptionResponseHandler>();
            services.AddTransient<IApiExceptionResponseHandler<RequiredPreconditionInvalidException>, RequiredPreconditionInvalidExceptionResponseHandler>();
        }
    }
}
