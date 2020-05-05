using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using CrudR.Api.Exceptions;
using CrudR.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CrudR.Api.ExceptionResponseHandlers
{
    /// <inheritdoc/>
    internal class ValidationExceptionResponseHandler : IApiExceptionResponseHandler<ValidationException>
    {
        /// <inheritdoc/>
        public ProblemDetails HandleResponse(ValidationException exception) =>
            new ProblemDetails
            {
                Title = "Data Validation Error",
                Detail = exception?.Message,
                Type = IetfStatusCodeTypes.BadRequestType,
                Status = (int)HttpStatusCode.BadRequest
            };

        /// <inheritdoc/>
        public ProblemDetails HandleResponse(Exception exception) =>
            HandleResponse((ValidationException)exception);
    }

    /// <inheritdoc/>
    internal class RecordNotFoundExceptionResponseHandler : IApiExceptionResponseHandler<RecordNotFoundException>
    {
        /// <inheritdoc/>
        public ProblemDetails HandleResponse(RecordNotFoundException exception) =>
            new ProblemDetails
            {
                Title = "Record Not Found Error",
                Type = IetfStatusCodeTypes.NotFoundType,
                Status = (int)HttpStatusCode.NotFound
            };

        /// <inheritdoc/>
        public ProblemDetails HandleResponse(Exception exception) =>
            HandleResponse((RecordNotFoundException)exception);
    }

    /// <inheritdoc/>
    internal class RecordNotModifiedExceptionResponseHandler : IApiExceptionResponseHandler<RecordNotModifiedException>
    {
        /// <inheritdoc/>
        public ProblemDetails HandleResponse(RecordNotModifiedException exception) =>
            new ProblemDetails
            {
                Title = "Record Not Modified Error",
                Type = IetfStatusCodeTypes.ConflictType,
                Status = (int)HttpStatusCode.Conflict
            };

        /// <inheritdoc/>
        public ProblemDetails HandleResponse(Exception exception) =>
            HandleResponse((RecordNotModifiedException)exception);
    }

    /// <inheritdoc/>
    internal class RecordAlreadyExistsExceptionResponseHandler : IApiExceptionResponseHandler<RecordAlreadyExistsException>
    {
        /// <inheritdoc/>
        public ProblemDetails HandleResponse(RecordAlreadyExistsException exception) =>
            new ProblemDetails
            {
                Title = "Record Already Exists Error",
                Detail = exception.Message,
                Type = IetfStatusCodeTypes.ForbiddenType,
                Status = (int)HttpStatusCode.Forbidden
            };

        /// <inheritdoc/>
        public ProblemDetails HandleResponse(Exception exception) =>
            HandleResponse((RecordAlreadyExistsException)exception);
    }

    /// <inheritdoc/>
    internal class RequiredPreconditionInvalidExceptionResponseHandler : IApiExceptionResponseHandler<RequiredPreconditionInvalidException>
    {
        /// <inheritdoc/>
        public ProblemDetails HandleResponse(RequiredPreconditionInvalidException exception) =>
            new ProblemDetails
            {
                Title = "Required Precondition Invalid Error",
                Type = IetfStatusCodeTypes.PreconditionRequiredType,
                Status = (int)HttpStatusCode.PreconditionRequired
            };

        /// <inheritdoc/>
        public ProblemDetails HandleResponse(Exception exception) =>
            HandleResponse((RequiredPreconditionInvalidException)exception);
    }
}
