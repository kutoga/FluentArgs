namespace FluentArgs.Validation
{
    public interface IValidation<in T>
    {
        bool IsValid(T value, out string? errorMessage);
    }

    internal interface IValidation : IValidation<object>
    {
    }

    internal static class IValidationExtensions
    {
        public static IValidation ToObjectValidation<T>(this IValidation<T> validation)
        {
            return CastedValidation.FromValidation(validation);
        }

        private class CastedValidation : IValidation
        {
            private readonly IsValidFunc wrappedValidation;

            private CastedValidation(IsValidFunc wrappedValidation)
            {
                this.wrappedValidation = wrappedValidation;
            }

            private delegate bool IsValidFunc(object value, out string? errorMessage);

            public static CastedValidation FromValidation<T>(IValidation<T> validation)
            {
                return new CastedValidation(IsValid);

                bool IsValid(object value, out string? errorMessage) =>
                    validation.IsValid((T)value, out errorMessage);
            }

            public bool IsValid(object value, out string? errorMessage)
            {
                return wrappedValidation(value, out errorMessage);
            }
        }
    }
}
