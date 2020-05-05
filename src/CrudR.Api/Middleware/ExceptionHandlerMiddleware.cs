using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using CrudR.Api.ExceptionResponseHandlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CrudR.Api.Middleware
{
    /// <summary>
    /// Central error/exception handler Middleware
    /// </summary>
    internal class ExceptionHandlerMiddleware
    {
        private const string ResponseContentType = "application/problem+json";

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlerMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="logger">The ILogger instance</param>
        /// <param name="serviceProvider">Dependency injection service provider</param>
        public ExceptionHandlerMiddleware(RequestDelegate next,
            ILogger<ExceptionHandlerMiddleware> logger,
            IServiceProvider serviceProvider)
        {
            _next = next;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Invoke(HttpContext context) => InvokeAsync(context);

        [SuppressMessage("Design", "CA1031:Do not catch general exception types",
            Justification = "This method is where generic Exceptions are handled for the application")]
        private async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                var problem = HandleExceptionTypes(exception);

                // set http status code and content type
                context.Response.StatusCode = problem.Status ?? 500;
                context.Response.ContentType = ResponseContentType;

                // writes / returns error model to the response
                await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
            }
        }

        /// <summary>
        /// Configurates/maps exception to the proper HTTP error Type
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        private ProblemDetails HandleExceptionTypes(Exception exception)
        {
            var problem = new ProblemDetails()
            {
                Type = IeftStatusCodeTypes.InternalServerErrorType,
                Title = "Internal Server Error",
                Status = (int)HttpStatusCode.InternalServerError,
            };
            var logLevel = LogLevel.Error;

            var responseHandlerType = typeof(IApiExceptionResponseHandler<>)
                .MakeGenericType(exception.GetType());

            var responseHandler = _serviceProvider.GetService(responseHandlerType) as IApiExceptionResponseHandler;
            if (responseHandler != null)
            {
                problem = responseHandler.HandleResponse(exception);
                logLevel = LogLevel.Debug;
            }

            problem.Extensions.Add("traceId", Activity.Current.Id);

            _logger.Log(logLevel, exception, exception.Message);

            return problem;
        }
    }
}
