namespace FluentArgs.Execution
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentArgs.Description;
    using FluentArgs.Validation;

    internal static class Validation
    {
        public static T ValidateIfRequired<T>(this T value, IValidator? validator, Name? argumentName = null)
        {
            if (validator != null && !validator.IsValid(value!, out var errorMessage))
            {
                if (string.IsNullOrEmpty(errorMessage))
                {
                    ThrowValidationError("Validation failed!");
                }
                else
                {
                    ThrowValidationError($"Validation failed: {errorMessage}");
                }

                void ThrowValidationError(string message) => throw new ArgumentParsingException(message, argumentName);
            }

            return value;
        }

        public static IEnumerable<T> ValidateIfRequired<T>(this IEnumerable<T> values, IValidator? validator, Name? argumentName = null)
        {
            return values.Select(ValidatedValue);

            T ValidatedValue(T value) => value.ValidateIfRequired(validator, argumentName);
        }
    }
}
