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

            [typeof(short)] = s => ParseNumber<short>(s, short.TryParse, short.TryParse),
            [typeof(int)] = s => ParseNumber<int>(s, int.TryParse, int.TryParse),
            [typeof(long)] = s => long.Parse(s, CultureInfo.InvariantCulture),

            [typeof(ushort)] = s => ushort.Parse(s, CultureInfo.InvariantCulture),
            [typeof(uint)] = s => uint.Parse(s, CultureInfo.InvariantCulture),
            [typeof(ulong)] = s => ulong.Parse(s, CultureInfo.InvariantCulture),

            [typeof(float)] = s => float.Parse(s, CultureInfo.InvariantCulture),
            [typeof(double)] = s => double.Parse(s, CultureInfo.InvariantCulture),
            [typeof(decimal)] = s => decimal.Parse(s, CultureInfo.InvariantCulture),

            //TODO: Does parsing 1, 0 usw. work?
            [typeof(bool)] = s => ParseBool(s), //bool.Parse(s),

            //TODO: Add date types etc.

            //TODO: test all parsers

            [typeof(DateTime)] = s => DateTime.Parse(s, CultureInfo.InvariantCulture),
            [typeof(DateTimeOffset)] = s => DateTimeOffset.Parse(s, CultureInfo.InvariantCulture),
            [typeof(TimeSpan)] = s => TimeSpan.Parse(s, CultureInfo.InvariantCulture),

            [typeof(Uri)] = s => new Uri(s)
        };

        private delegate bool TryParseNumber<T>(string input, out T result);

        private delegate bool TryParseNumberWithFormat<T>(string input, NumberStyles style, IFormatProvider provider, out T result);

        private static T ParseNumber<T>(string input, TryParseNumber<T> tryParse, TryParseNumberWithFormat<T> tryparseWithFormat)
        {
            T result;

            if (tryParse(input, out result))
            {
                return result;
            }

            if (input.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) && tryparseWithFormat(input, NumberStyles.HexNumber,
                    CultureInfo.InvariantCulture, out result))
            {
                return result;
            }

            throw new FormatException($"Cannot parse '{input}' as type '{typeof(T).Name}'!");
        }

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

        private static bool TryGetNullableTypeParser(Type targetType, out Func<string, object>? parser)
        {
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var wrappedType = targetType.GenericTypeArguments[0];
                if (Parsers.ContainsKey(wrappedType))
                {
                    parser = Parsers[wrappedType];
                    return true;
                }
            }

            parser = default;
            return false;
        }

        private static bool ParseBool(string s)
        {
            var sLower = s.ToLowerInvariant();
            if (bool.TryParse(sLower, out bool result))
            {
                return result;
            }

            if (sLower == "yes" || sLower == "y" || sLower == "1" || sLower == "true")
            {
                return true;
            }

            if (sLower == "no" || sLower == "n" || sLower == "0" || sLower == "false")
            {
                return false;
            }

            throw new ArgumentException($"Cannot parse boolean '{s}'!");
        }

        public static bool TryGetParser(Type targetType, out Func<string, object>? parser)
        {
            if (Parsers.ContainsKey(targetType))
            {
                parser = Parsers[targetType];
                return true;
            }

            if (TryGetNullableTypeParser(targetType, out parser))
            {
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
