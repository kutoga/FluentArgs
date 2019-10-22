namespace FluentArgs
{
    using System;
    using FluentArgs.Validation;

    public interface IWithConfigurableValidation<TBase, TParam>
    {
        TBase WithValidation(IValidation<TParam> validation);
    }

    public static class IWithConfigurableValidationExtensions
    {
        public delegate bool ValidationFunc<T>(T obj, out string? errorMessage);

        public static TBase WithValidation<TBase, TParam>(this IWithConfigurableValidation<TBase, TParam> withConfigurableValidation, Func<TParam, bool> validation, string? errorMessage = null)
        {
            return withConfigurableValidation.WithValidation(validation, _ => errorMessage);
        }

        public static TBase WithValidation<TBase, TParam>(this IWithConfigurableValidation<TBase, TParam> withConfigurableValidation, Func<TParam, bool> validation, Func<TParam, string?> errorMessageGenerator)
        {
            return withConfigurableValidation.WithValidation(Validation);

            bool Validation(TParam value, out string? errorMessage)
            {
                var result = validation(value);
                if (result)
                {
                    errorMessage = errorMessageGenerator(value);
                }
                else
                {
                    errorMessage = default;
                }

                return result;
            }
        }

        public static TBase WithValidation<TBase, TParam>(this IWithConfigurableValidation<TBase, TParam> withConfigurableValidation, ValidationFunc<TParam> validationFunc)
        {
            return withConfigurableValidation.WithValidation(new SimpleValidation<TParam>(validationFunc));
        }

        private class SimpleValidation<T> : IValidation<T>
        {
            private readonly ValidationFunc<T> validationFunc;

            public SimpleValidation(ValidationFunc<T> validationFunc)
            {
                this.validationFunc = validationFunc ?? throw new ArgumentNullException(nameof(validationFunc));
            }

            public bool IsValid(T value, out string? errorMessage)
            {
                return validationFunc(value, out errorMessage);
            }
        }
    }
}
