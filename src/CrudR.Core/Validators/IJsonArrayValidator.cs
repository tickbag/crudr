using System.Text.Json;
using CrudR.Core.Validators.Models;

namespace CrudR.Core.Validators
{
    /// <summary>
    /// Json schema validator for an Array object.
    /// The validator should iterate the array checking each object for a schema match.
    /// </summary>
    internal interface IJsonArrayValidator
    {
        /// <summary>
        /// Validate an input srray agaist a source object/array
        /// </summary>
        /// <param name="input">An input JsonElement representing an Array of objects</param>
        /// <param name="source">A source JsonElement representing an object</param>
        /// <returns>A ValidationResult of true if the schemas match</returns>
        ValidationResult Validate(JsonElement input, JsonElement source);
    }
}
