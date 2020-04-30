using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using CrudR.Core.Models;

namespace CrudR.Core.Validators
{
    /// <summary>
    /// Store Model Validator for comparing a stored Json model to the schema of a new Json model
    /// </summary>
    internal class StoreModelValidator : IStoreModelValidator
    {
        private readonly IJsonArrayValidator _jsonArrayValidator;
        private readonly IJsonObjectValidator _jsonObjectValidator;

        public StoreModelValidator(IJsonArrayValidator jsonArrayValidator, IJsonObjectValidator jsonObjectValidator)
        {
            _jsonArrayValidator = jsonArrayValidator;
            _jsonObjectValidator = jsonObjectValidator;
        }

        /// <inheritdoc/>
        public void Validate(StoreModel input, StoreModel source)
        {
            _ = input ?? throw new ArgumentNullException(nameof(input));
            _ = source ?? throw new ArgumentNullException(nameof(source));

            var validationResult = input.Payload.ValueKind == JsonValueKind.Array ?
                _jsonArrayValidator.Validate(input.Payload, source.Payload) :
                _jsonObjectValidator.Validate(input.Payload, source.Payload);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }
    }
}
