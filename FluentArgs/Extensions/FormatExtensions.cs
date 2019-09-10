using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentArgs.Description;

namespace FluentArgs.Extensions
{
    internal static class FormatExtensions
    {
        public static string StringifyAliases(this IReadOnlyCollection<string> aliases, string separator = "|")
        {
            var orderedNames = aliases.OrderBy(a => a.Length).ThenBy(a => a);
            return string.Join(separator, orderedNames);
        }
    }
}
