namespace FluentArgs
{
    using System.Globalization;
    using System.Linq;

    public interface IConfigurableParameterList<TArgsBuilder, TParam> : IConfigurableParameter<TArgsBuilder, TParam>
    {
        IConfigurableParameterList<TArgsBuilder, TParam> WithSeparator(string separator, params string[] moreSeparators);
    }

    public static class IConfigurableParameterListExtensions
    {
        public static IConfigurableParameterList<TArgsBuilder, TParam> WithSeparator<TArgsBuilder, TParam>(this IConfigurableParameterList<TArgsBuilder, TParam> configurableParameterList, char separator, params char[] moreSeparators)
        {
            return configurableParameterList.WithSeparator(separator.ToString(CultureInfo.InvariantCulture), moreSeparators.Select(c => c.ToString(CultureInfo.InvariantCulture)).ToArray());
        }
    }
}
