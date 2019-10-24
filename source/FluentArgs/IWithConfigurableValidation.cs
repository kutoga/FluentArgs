namespace FluentArgs
{
    using System;
    using FluentArgs.Validation;

    public interface IWithConfigurableValidation<TBase, TParam>
    {
        TBase WithValidation(ValidationFunc<TParam> validation);
    }

    public static class IWithConfigurableValidationExtensions
    {
        public static TBase WithValidation<TBase, TParam>(this IWithConfigurableValidation<TBase, TParam> withConfigurableValidation, Func<TParam, bool> validation, string? errorMessage = null)
        {
            return withConfigurableValidation.WithValidation(validation, _ => errorMessage);
        }

        public static TBase WithValidation<TBase, TParam>(this IWithConfigurableValidation<TBase, TParam> withConfigurableValidation, Func<TParam, bool> validation, Func<TParam, string?> errorMessageGenerator)
        {
            return withConfigurableValidation.WithValidation((TParam value, out string? errorMessage) =>
            {
                var result = validation(value);
                errorMessage = result ? default : errorMessageGenerator(value);
                return result;
            });
        }
    }
}
