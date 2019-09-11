using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentArgs.Description;

namespace FluentArgs.Extensions
{
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
