namespace CrudR.Core.Validators.Models
{
    /// <summary>
    /// A Validation result instance for use by internal JSON schema validators
    /// </summary>
    internal class ValidationResult
    {
        private const string NewLine = "\n";

        public ValidationResult(bool isValid, string errors) =>
            (IsValid, Errors) = (isValid, errors);

        public ValidationResult(bool isValid) : this(isValid, null) { }

        public string Errors { get; }

        public bool IsValid { get; }

        public static ValidationResult operator +(ValidationResult a, ValidationResult b)
        {
            if (a == null && b == null)
                return null;

            a ??= new ValidationResult(true);
            b ??= new ValidationResult(true);

            return new ValidationResult(
                a.IsValid && b.IsValid,
                string.IsNullOrEmpty(a.Errors) || string.IsNullOrEmpty(b.Errors) ?
                    string.Concat(a.Errors, b.Errors) :
                    string.Join(NewLine, new[] { a.Errors, b.Errors }));
        }
    }
}
