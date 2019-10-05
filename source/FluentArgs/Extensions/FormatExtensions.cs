namespace FluentArgs.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    internal static class FormatExtensions
    {
        public static IEnumerable<string> AliasesOrdering(this IEnumerable<string> aliases)
        {
            return aliases.OrderBy(a => a.Length).ThenBy(a => a);
        }

        public static string StringifyAliases(this IReadOnlyCollection<string> aliases, string separator = "|")
        {
            return string.Join(separator, aliases.AliasesOrdering());
        }
    }
}
