namespace FluentArgs.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Globalization;
    using System.Linq;

    internal static class DefaultStringParsers
    {
        private static readonly IReadOnlyDictionary<Type, Func<string, object>> Parsers = new Dictionary<Type, Func<string, object>>
        {
            [typeof(string)] = s => s,

            [typeof(short)] = s => short.Parse(s, CultureInfo.InvariantCulture),
            [typeof(int)] = s => int.Parse(s, CultureInfo.InvariantCulture),
            [typeof(long)] = s => long.Parse(s, CultureInfo.InvariantCulture),

            [typeof(ushort)] = s => ushort.Parse(s, CultureInfo.InvariantCulture),
            [typeof(uint)] = s => uint.Parse(s, CultureInfo.InvariantCulture),
            [typeof(ulong)] = s => ulong.Parse(s, CultureInfo.InvariantCulture),

            [typeof(float)] = s => float.Parse(s, CultureInfo.InvariantCulture),
            [typeof(double)] = s => double.Parse(s, CultureInfo.InvariantCulture),
            [typeof(decimal)] = s => decimal.Parse(s, CultureInfo.InvariantCulture),

            //TODO: Does parsing 1, 0 usw. work?
            [typeof(bool)] = s => bool.Parse(s),

            //TODO: Add date types etc.

            //TODO: test all parsers

            [typeof(DateTime)] = s => DateTime.Parse(s, CultureInfo.InvariantCulture),
            [typeof(DateTimeOffset)] = s => DateTimeOffset.Parse(s, CultureInfo.InvariantCulture),
            [typeof(TimeSpan)] = s => TimeSpan.Parse(s, CultureInfo.InvariantCulture),

            [typeof(Uri)] = s => new Uri(s)
        };

        private static bool TryGetEnumParser(Type enumType, out Func<string, object>? parser)
        {
            if (!enumType.IsSubclassOf(typeof(Enum)))
            {
                parser = default;
                return false;
            }

            var names = Enum.GetNames(enumType).ToImmutableHashSet();
            object Parse(string name)
            {
                if (names.Contains(name))
                {
                    return Enum.Parse(enumType, name);
                }

                var upperName = name.ToUpperInvariant();
                var caseInsensitiveMatchingNames = names.Where(n => n.ToUpperInvariant() == upperName).ToList();
                if (caseInsensitiveMatchingNames.Count == 1)
                {
                    return Enum.Parse(enumType, caseInsensitiveMatchingNames[0]);
                }

                throw new ArgumentException($"Invalid value '{name}' for the type '{enumType.Name}'!");
            }

            parser = Parse;
            return true;
        }

        public static bool TryGetParser(Type targetType, out Func<string, object>? parser)
        {
            if (Parsers.ContainsKey(targetType))
            {
                parser = Parsers[targetType];
                return true;
            }

            if (TryGetEnumParser(targetType, out parser))
            {
                return true;
            }

            parser = default;
            return false;
        }
    }
}
