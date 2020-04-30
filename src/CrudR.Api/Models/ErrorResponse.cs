namespace CrudR.Api.Models
{
    /// <summary>
    /// The Error Response model
    /// </summary>
    internal class ErrorResponse
    {
        /// <summary>
        /// The Error Response constructor
        /// </summary>
        /// <param name="message">Error message</param>
        public ErrorResponse(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Error message to display in the response
        /// </summary>
        public string Message { get; }
    }
}
