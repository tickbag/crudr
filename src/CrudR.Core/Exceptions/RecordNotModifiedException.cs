using System;

namespace CrudR.Core.Exceptions
{
    /// <summary>
    /// Exception indicating that a record was not modified (usually due to a data consistency conflict)
    /// </summary>
    public class RecordNotModifiedException : Exception
    {
        /// <summary>
        /// Create a default instance of the exception
        /// </summary>
        public RecordNotModifiedException()
        {
        }

        /// <summary>
        /// Create the exception with the provided message
        /// </summary>
        /// <param name="message">Exception message</param>
        public RecordNotModifiedException(string message) : base(message)
        {
        }

        /// <summary>
        /// Create the exception with a message and an inner exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public RecordNotModifiedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
