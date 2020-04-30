using CrudR.Core.Models;

namespace CrudR.Core.Validators
{
    /// <summary>
    /// Store Model Validator for comparing a stored Json model to the schema of a new Json model
    /// </summary>
    public interface IStoreModelValidator
    {
        /// <summary>
        /// Validate that the first level of Json schema for the two supplied models matches.
        /// </summary>
        /// <param name="input">New Store Model to validate</param>
        /// <param name="source">Currently stored Store Model to validate against</param>
        void Validate(StoreModel input, StoreModel source);
    }
}
