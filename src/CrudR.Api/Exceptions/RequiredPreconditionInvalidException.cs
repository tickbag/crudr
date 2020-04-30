using System;

namespace CrudR.Api.Exceptions
{
    /// <summary>
    /// Exception indicating that a required request precondition is either missing or invalid
    /// </summary>
    public class RequiredPreconditionInvalidException : Exception
    {
        /// <summary>
        /// Create a default instance of the exception
        /// </summary>
        public RequiredPreconditionInvalidException()
        {
        }

        /// <summary>
        /// Create the exception with the provided message
        /// </summary>
        /// <param name="message">Exception message</param>
        public RequiredPreconditionInvalidException(string message) : base(message)
        {
        }

        /// <summary>
        /// Create the exception with a message and an inner exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public RequiredPreconditionInvalidException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
