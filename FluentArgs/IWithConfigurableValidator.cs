namespace FluentArgs
{
    using System;
    using FluentArgs.Validation;

    public interface IWithConfigurableValidator<TBase, TParam>
    {
        TBase WithValidator(IValidator<TParam> validator);
    }

    public static class IWithConfigurableValidatorExtensions
    {
        public static TBase WithValidator<TBase, TParam>(this IWithConfigurableValidator<TBase, TParam> withConfigurableValidator, Func<TParam, bool> validator, string? errorMessage = null)
        {
            return withConfigurableValidator.WithValidator(validator, _ => errorMessage);
        }

        public static TBase WithValidator<TBase, TParam>(this IWithConfigurableValidator<TBase, TParam> withConfigurableValidator, Func<TParam, bool> validator, Func<TParam, string?> errorMessageGenerator)
        {
            return withConfigurableValidator.WithValidator(new SimpleValidator<TParam>(validator, errorMessageGenerator));
        }

        private class SimpleValidator<T> : IValidator<T>
        {
            private readonly Func<T, bool> validator;
            private readonly Func<T, string?> errorMessageGenerator;

            public SimpleValidator(Func<T, bool> validator, Func<T, string?> errorMessageGenerator)
            {
                this.validator = validator;
                this.errorMessageGenerator = errorMessageGenerator;
            }

            public bool IsValid(T value, out string? errorMessage)
            {
                bool valid = validator(value);
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
