using System.Text.Json;
using CrudR.Core.Validators.Models;

namespace CrudR.Core.Validators
{
    internal class JsonObjectValidator : IJsonObjectValidator
    {
        private static ValidationResult InputDataNotAnObjectResult() =>
            new ValidationResult(false, "Input Json data definition is not an Object.");

        private static ValidationResult StoredDataNotAnObjectResult() =>
            new ValidationResult(false, "Stored data definition is not an Object.");

        public ValidationResult Validate(JsonElement input, JsonElement source)
        {
            if (input.ValueKind != JsonValueKind.Object)
                return InputDataNotAnObjectResult();

            if (source.ValueKind != JsonValueKind.Object)
                return StoredDataNotAnObjectResult();

            return CheckInputAgainstSource(input, source) +
                CheckSourceAgainstInput(input, source);
        }

        private ValidationResult CheckInputAgainstSource(JsonElement input, JsonElement stored)
        {
            var result = new ValidationResult(true);

            foreach (var element in input.EnumerateObject())
            {
                var value = new JsonElement();
                result += ValidationEvaluator.Evaluate(() => stored.TryGetProperty(element.Name, out value),
                    $"Json property '{element.Name}' is missing from the stored data definition.",
                    () => ValidationEvaluator.Evaluate(() => element.Value.ValueKind == value.ValueKind,
                    $"Json property {element.Name} has a differing data type.",
                    () => new ValidationResult(true)));
            }

            return result;
        }

        private ValidationResult CheckSourceAgainstInput(JsonElement input, JsonElement source)
        {
            var result = new ValidationResult(true);

            foreach (var element in source.EnumerateObject())
            {
                result += ValidationEvaluator.Evaluate(() => input.TryGetProperty(element.Name, out _),
                    $"Json property '{element.Name}' is missing from the input model.",
                    () => new ValidationResult(true));
            }

            return result;
        }
    }
}
