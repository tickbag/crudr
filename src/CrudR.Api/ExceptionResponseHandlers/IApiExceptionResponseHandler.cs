using System;
using Microsoft.AspNetCore.Mvc;

namespace CrudR.Api.ExceptionResponseHandlers
{
    /// <summary>
    /// Defines a handler to deal with generating Api response models
    /// </summary>
    public interface IApiExceptionResponseHandler
    {
        /// <summary>
        /// Generates the response model for this exception type
        /// </summary>
        /// <param name="exception">The exception</param>
        /// <returns>A ProblemDetails object containing the error information</returns>
        ProblemDetails HandleResponse(Exception exception);
    }

    /// <summary>
    /// Defines a handler to deal with generating Api response models for a specific Exception type
    /// </summary>
    /// <typeparam name="T">Type of Exception this handler will deal with</typeparam>
    public interface IApiExceptionResponseHandler<T> : IApiExceptionResponseHandler where T : Exception
    {
        /// <summary>
        /// Generates the response model for this exception type
        /// </summary>
        /// <param name="exception">The exception</param>
        /// <returns>A ProblemDetails object containing the error information</returns>
        ProblemDetails HandleResponse(T exception);
    }
}
