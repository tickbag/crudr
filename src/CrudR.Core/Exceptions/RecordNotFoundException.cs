using System;

namespace CrudR.Core.Exceptions
{
    /// <summary>
    /// Exception indicating that a record was not found
    /// </summary>
    public class RecordNotFoundException : Exception
    {
        /// <summary>
        /// Create a default instance of the exception
        /// </summary>
        public RecordNotFoundException()
        {
        }

        /// <summary>
        /// Create the exception with a provided message
        /// </summary>
        /// <param name="message">Exception message</param>
        public RecordNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Create the exception with a message and an inner exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public RecordNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
