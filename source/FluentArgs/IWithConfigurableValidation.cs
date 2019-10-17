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
        public static TBase WithValidation<TBase, TParam>(this IWithConfigurableValidation<TBase, TParam> withConfigurableValidation, Func<TParam, bool> validation, string? errorMessage = null)
        {
            return withConfigurableValidation.WithValidation(validation, _ => errorMessage);
        }

        public static TBase WithValidation<TBase, TParam>(this IWithConfigurableValidation<TBase, TParam> withConfigurableValidation, Func<TParam, bool> validation, Func<TParam, string?> errorMessageGenerator)
        {
            return withConfigurableValidation.WithValidation(new SimpleValidation<TParam>(validation, errorMessageGenerator));
        }

        private class SimpleValidation<T> : IValidation<T>
        {
            private readonly Func<T, bool> validation;
            private readonly Func<T, string?> errorMessageGenerator;

            public SimpleValidation(Func<T, bool> validation, Func<T, string?> errorMessageGenerator)
            {
                this.validation = validation;
                this.errorMessageGenerator = errorMessageGenerator;
            }

            public bool IsValid(T value, out string? errorMessage)
            {
                bool valid = validation(value);
                if (!valid)
                {
                    errorMessage = errorMessageGenerator(value);
                }
                else
                {
                    errorMessage = default;
                }

                return valid;
            }
        }
    }
}
