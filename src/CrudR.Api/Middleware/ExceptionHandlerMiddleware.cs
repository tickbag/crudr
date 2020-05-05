using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using CrudR.Api.Exceptions;
using CrudR.Core.Exceptions;
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
            problem.Extensions.Add("traceId", Activity.Current.Id);

            // Exception type To Http Status configuration 
            switch (exception)
            {
                case var _ when exception is ValidationException:
                    problem.Title = "Data Validation Error";
                    problem.Detail = exception.Message;
                    problem.Type = IeftStatusCodeTypes.BadRequestType;
                    problem.Status = (int)HttpStatusCode.BadRequest;
                    _logger.LogDebug(exception, exception.Message);
                    break;
                case var _ when exception is RecordNotFoundException:
                    problem.Title = "Record Not Found Error";
                    problem.Type = IeftStatusCodeTypes.NotFoundType;
                    problem.Status = (int)HttpStatusCode.NotFound;
                    _logger.LogDebug(exception, exception.Message);
                    break;
                case var _ when exception is RecordNotModifiedException:
                    problem.Title = "Record Not Modified Error";
                    problem.Type = IeftStatusCodeTypes.ConflictType;
                    problem.Status = (int)HttpStatusCode.Conflict;
                    _logger.LogDebug(exception, exception.Message);
                    break;
                case var _ when exception is RecordAlreadyExistsException:
                    problem.Title = "Record Already Exists Error";
                    problem.Detail = exception.Message;
                    problem.Type = IeftStatusCodeTypes.ForbiddenType;
                    problem.Status = (int)HttpStatusCode.Forbidden;
                    _logger.LogDebug(exception, exception.Message);
                    break;
                case var _ when exception is RequiredPreconditionInvalidException:
                    problem.Title = "Required Precondition Invalid Error";
                    problem.Type = IeftStatusCodeTypes.PreconditionRequiredType;
                    problem.Status = (int)HttpStatusCode.PreconditionRequired;
                    _logger.LogDebug(exception, exception.Message);
                    break;
                default:
                    problem.Detail = exception.Message;
                    _logger.LogError(exception, exception.Message);
                    break;
            }

            return problem;
        }
    }
}
