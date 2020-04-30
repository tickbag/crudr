using System.Text.Json;
using CrudR.Core.Validators.Models;

namespace CrudR.Core.Validators
{
    /// <summary>
    /// Json schema validator for an object.
    /// The validator should check each object for a schema match.
    /// </summary>
    internal interface IJsonObjectValidator
    {
        /// <summary>
        /// Validate an input object agaist a source object
        /// </summary>
        /// <param name="input">An input JsonElement representing an object</param>
        /// <param name="source">A source JsonElement representing an object</param>
        /// <returns>A ValidationResult of true if the schemas match</returns>
        ValidationResult Validate(JsonElement input, JsonElement source);
    }
}
