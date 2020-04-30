using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using CrudR.Api.Exceptions;
using CrudR.Api.Models;
using CrudR.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CrudR.Api.Middleware
{
    /// <summary>
    /// Central error/exception handler Middleware
    /// </summary>
    internal class ExceptionHandlerMiddleware
    {
        private const string ResponseContentType = "application/json";
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlerMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="logger">The ILogger instance</param>
        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
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
                (var httpStatusCode, var errorResponse) = HandleExceptionTypes(exception);

                // set http status code and content type
                context.Response.StatusCode = httpStatusCode;
                context.Response.ContentType = ResponseContentType;

                // writes / returns error model to the response
                if (errorResponse != null)
                    await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
        }

        /// <summary>
        /// Configurates/maps exception to the proper HTTP error Type
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        private (int, ErrorResponse) HandleExceptionTypes(Exception exception)
        {
            int httpStatusCode;
            ErrorResponse errorResponse = null;

            // Exception type To Http Status configuration 
            switch (exception)
            {
                case var _ when exception is ValidationException:
                    httpStatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse = new ErrorResponse(exception.Message);
                    _logger.LogDebug(exception, exception.Message);
                    break;
                case var _ when exception is RecordNotFoundException:
                    httpStatusCode = (int)HttpStatusCode.NotFound;
                    _logger.LogDebug(exception, exception.Message);
                    break;
                case var _ when exception is RecordNotModifiedException:
                    httpStatusCode = (int)HttpStatusCode.Conflict;
                    _logger.LogDebug(exception, exception.Message);
                    break;
                case var _ when exception is RecordAlreadyExistsException:
                    httpStatusCode = (int)HttpStatusCode.Forbidden;
                    errorResponse = new ErrorResponse(exception.Message);
                    _logger.LogDebug(exception, exception.Message);
                    break;
                case var _ when exception is RequiredPreconditionInvalidException:
                    httpStatusCode = (int)HttpStatusCode.PreconditionRequired;
                    _logger.LogDebug(exception, exception.Message);
                    break;
                default:
                    httpStatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse = new ErrorResponse(exception.Message);
                    _logger.LogError(exception, exception.Message);
                    break;
            }

            return (httpStatusCode, errorResponse);
        }
    }
}
