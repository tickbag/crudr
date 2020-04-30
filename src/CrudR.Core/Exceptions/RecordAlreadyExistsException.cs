using System;

namespace CrudR.Core.Exceptions
{
    /// <summary>
    /// Exception indicating that a record already exists with that Id
    /// </summary>
    public class RecordAlreadyExistsException : Exception
    {
        private const string DefaultMessage = "A record already exists at that location.";

        /// <summary>
        /// Create the exception with a default message
        /// </summary>
        public RecordAlreadyExistsException() : base(DefaultMessage)
        {
        }

        /// <summary>
        /// Create the exception with the provided message
        /// </summary>
        /// <param name="message">Exception message</param>
        public RecordAlreadyExistsException(string message) : base(message)
        {
        }

        /// <summary>
        /// Create the exception with a message and inner exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public RecordAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
