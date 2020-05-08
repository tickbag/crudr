using System;
using System.ComponentModel.DataAnnotations;
using CrudR.Api.Exceptions;
using Microsoft.Extensions.Configuration;

namespace CrudR.Api.Extensions
{
    /// <summary>
    /// Configuration object extension methods
    /// </summary>
    internal static class ConfigurationExtensions
    {
        private const bool ValidateAllProperties = true;

        /// <summary>
        /// Validate a strongly typed configuration object
        /// </summary>
        /// <typeparam name="T">The strongly typed configuration class to validate against</typeparam>
        /// <param name="configuration">The configuration section to load into the class</param>
        /// <returns>A valid configuration class</returns>
        public static T GetValidated<T>(this IConfiguration configuration)
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            try
            {
                var obj = configuration.Get<T>() ?? (T)Activator.CreateInstance(typeof(T));
                Validator.ValidateObject(obj, new ValidationContext(obj), ValidateAllProperties);
                return obj;
            }
            catch (ValidationException vex)
            {
                throw new ConfigurationValidationException($"Configuration section {typeof(T).Name} has an invalid setting", vex);
            }
        }
    }
}
