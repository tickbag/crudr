using System.Linq;
using System.Text.Json;
using CrudR.Core.Validators.Models;

namespace CrudR.Core.Validators
{
    internal class JsonArrayValidator : IJsonArrayValidator
    {
        private static ValidationResult InputDataNotAnArrayResult() =>
            new ValidationResult(false, "Input Json data definition is not an Array.");

        private static ValidationResult StoredDataNotAnArrayResult() =>
            new ValidationResult(false, "Stored data definition is not an Array.");

        private static ValidationResult InputDataArrayIsEmptyResult() =>
            new ValidationResult(false, "Input data array is empty");

        private readonly IJsonObjectValidator _jsonObjectValidator;

        public JsonArrayValidator(IJsonObjectValidator jsonObjectValidator)
        {
            _jsonObjectValidator = jsonObjectValidator;
        }

        public ValidationResult Validate(JsonElement input, JsonElement source)
        {
            if (input.ValueKind != JsonValueKind.Array)
                return InputDataNotAnArrayResult();

            if (source.ValueKind != JsonValueKind.Array)
                return StoredDataNotAnArrayResult();

            var storedJsonObject = source.EnumerateArray().FirstOrDefault();

            var result = InputDataArrayIsEmptyResult();
            foreach (var inputJsonObject in input.EnumerateArray())
            {
                result = _jsonObjectValidator.Validate(inputJsonObject, storedJsonObject);
                if (!result.IsValid)
                    break;
            }

            return result;
        }
    }
}
