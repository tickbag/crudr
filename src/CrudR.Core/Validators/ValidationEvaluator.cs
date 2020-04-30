using System;
using CrudR.Core.Validators.Models;

namespace CrudR.Core.Validators
{
    internal static class ValidationEvaluator
    {
        public static ValidationResult Evaluate(Func<bool> evaluationFunction, string errorMessage, Func<ValidationResult> onSuccess)
        {
            var isValid = evaluationFunction.Invoke();

            return isValid ? onSuccess.Invoke() :
                new ValidationResult(isValid, errorMessage);
        }
    }
}
