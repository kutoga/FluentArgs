namespace FluentArgs
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public interface IConfigurableListParameter<TArgsBuilder, TParam>
        : IWithConfigurableValidation<IConfigurableListParameter<TArgsBuilder, TParam>, TParam>
    {
        IConfigurableListParameter<TArgsBuilder, TParam> WithDescription(string description);

        IConfigurableListParameter<TArgsBuilder, TParam> WithExamples(IReadOnlyCollection<TParam> example, params IReadOnlyCollection<TParam>[] moreExamples);

        IConfigurableListParameter<TArgsBuilder, TParam> WithExamples(string example, params string[] moreExamples);

        IConfigurableListParameter<TArgsBuilder, TParam> WithParser(Func<string, TParam> parser);

        TArgsBuilder IsOptional();

        TArgsBuilder IsOptionalWithDefault(IReadOnlyCollection<TParam> defaultValue);

        TArgsBuilder IsRequired();

        IConfigurableListParameter<TArgsBuilder, TParam> WithSeparator(string separator, params string[] moreSeparators);

        TArgsBuilder IsOptionalWithEmptyDefault();
    }

    public static class IConfigurableListParameterExtensions
    {
        public static IConfigurableListParameter<TArgsBuilder, TParam> WithSeparator<TArgsBuilder, TParam>(this IConfigurableListParameter<TArgsBuilder, TParam> configurableListParameter, char separator, params char[] moreSeparators)
        {
            return configurableListParameter.WithSeparator(separator.ToString(CultureInfo.InvariantCulture), moreSeparators.Select(c => c.ToString(CultureInfo.InvariantCulture)).ToArray());
        }
    }
}
