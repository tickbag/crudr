namespace CrudR.Api.ExceptionResponseHandlers
{
    /// <summary>
    /// A collection IEFT Statuc Code URI type scpecifications
    /// </summary>
    public static class IetfStatusCodeTypes
    {
        /// <summary>
        /// 500 - Internal Server Error
        /// </summary>
        public const string InternalServerErrorType = "https://tools.ietf.org/html/rfc7231#section-6.6.1";

        /// <summary>
        /// 400 - Bad Request
        /// </summary>
        public const string BadRequestType = "https://tools.ietf.org/html/rfc7231#section-6.5.1";

        /// <summary>
        /// 403 - Forbidden
        /// </summary>
        public const string ForbiddenType = "https://tools.ietf.org/html/rfc7231#section-6.5.3";

        /// <summary>
        /// 404 - Not Found
        /// </summary>
        public const string NotFoundType = "https://tools.ietf.org/html/rfc7231#section-6.5.4";

        /// <summary>
        /// 409 - Conflict
        /// </summary>
        public const string ConflictType = "https://tools.ietf.org/html/rfc7231#section-6.5.8";

        /// <summary>
        /// 428 - Precondition Required
        /// </summary>
        public const string PreconditionRequiredType = "https://tools.ietf.org/html/rfc6585#section-3";
    }
}
