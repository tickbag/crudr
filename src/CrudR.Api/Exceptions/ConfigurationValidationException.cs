using System;
using System.Runtime.Serialization;

namespace CrudR.Api.Exceptions
{
    /// <summary>
    /// Throw this exception when a configuration setting is invalid
    /// </summary>
    public class ConfigurationValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationValidationException"/> class.
        /// </summary>
        public ConfigurationValidationException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationValidationException"/> class.
        /// </summary>
        /// <param name="message">Configuration validation error message</param>
        public ConfigurationValidationException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationValidationException"/> class.
        /// </summary>
        /// <param name="message">Configuration validation error message</param>
        /// <param name="innerException">Inner exception to include</param>
        public ConfigurationValidationException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationValidationException"/> class.
        /// </summary>
        /// <param name="info">Serialisation info</param>
        /// <param name="context">Streaming context</param>
        protected ConfigurationValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
