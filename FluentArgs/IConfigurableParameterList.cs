namespace FluentArgs
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public interface IConfigurableParameterList<TArgsBuilder, TParam>
        : IWithConfigurableValidator<IConfigurableParameterList<TArgsBuilder, TParam>, TParam>
    {
        IConfigurableParameterList<TArgsBuilder, TParam> WithDescription(string description);

        IConfigurableParameterList<TArgsBuilder, TParam> WithExamples(IReadOnlyCollection<TParam> example, params IReadOnlyCollection<TParam>[] moreExamples);

        IConfigurableParameterList<TArgsBuilder, TParam> WithExamples(string example, params string[] moreExamples);

        IConfigurableParameterList<TArgsBuilder, TParam> WithParser(Func<string, TParam> parser);

        TArgsBuilder IsOptional();

        TArgsBuilder IsOptionalWithDefault(IReadOnlyCollection<TParam> defaultValue);

        TArgsBuilder IsRequired();

        IConfigurableParameterList<TArgsBuilder, TParam> WithSeparator(string separator, params string[] moreSeparators);

        TArgsBuilder IsOptionalWithEmptyDefault();
    }

    public static class IConfigurableParameterListExtensions
    {
        public static IConfigurableParameterList<TArgsBuilder, TParam> WithSeparator<TArgsBuilder, TParam>(this IConfigurableParameterList<TArgsBuilder, TParam> configurableParameterList, char separator, params char[] moreSeparators)
        {
            return configurableParameterList.WithSeparator(separator.ToString(CultureInfo.InvariantCulture), moreSeparators.Select(c => c.ToString(CultureInfo.InvariantCulture)).ToArray());
        }
    }
}
