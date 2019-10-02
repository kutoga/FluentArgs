namespace FluentArgs.Validation
{
    public interface IValidator<in T>
    {
        bool IsValid(T value, out string? errorMessage);
    }

    internal interface IValidator : IValidator<object>
    {
    }

    internal static class IValidatorExtensions
    {
        public static IValidator ToObjectValidator<T>(this IValidator<T> validator)
        {
            return CastedValidator.FromValidator(validator);
        }

        private class CastedValidator : IValidator
        {
            private delegate bool IsValidFunc(object value, out string? errorMessage);

            private readonly IsValidFunc wrappedValidator;

            private CastedValidator(IsValidFunc wrappedValidator)
            {
                this.wrappedValidator = wrappedValidator;
            }

            public bool IsValid(object value, out string? errorMessage)
            {
                return wrappedValidator(value, out errorMessage);
            }

            public static CastedValidator FromValidator<T>(IValidator<T> validator)
            {
                return new CastedValidator(IsValid);

                bool IsValid(object value, out string? errorMessage) =>
                    validator.IsValid((T)value, out errorMessage);
            }
        }
    }
}
