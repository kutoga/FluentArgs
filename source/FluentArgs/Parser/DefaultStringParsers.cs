namespace FluentArgs.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    internal static class DefaultStringParsers
    {
        private static readonly IReadOnlyDictionary<Type, Func<string, object>> Parsers = new Dictionary<Type, Func<string, object>>
        {
            [typeof(string)] = s => s,

            [typeof(short)] = s => ParseNumber<short>(s, short.Parse, short.TryParse),
            [typeof(int)] = s => ParseNumber<int>(s, int.Parse, int.TryParse),
            [typeof(long)] = s => ParseNumber<long>(s, long.Parse, long.TryParse),

            [typeof(ushort)] = s => ParseNumber<ushort>(s, ushort.Parse, ushort.TryParse),
            [typeof(uint)] = s => ParseNumber<uint>(s, uint.Parse, uint.TryParse),
            [typeof(ulong)] = s => ParseNumber<ulong>(s, ulong.Parse, ulong.TryParse),

            [typeof(float)] = s => float.Parse(s, CultureInfo.InvariantCulture),
            [typeof(double)] = s => double.Parse(s, CultureInfo.InvariantCulture),
            [typeof(decimal)] = s => decimal.Parse(s, CultureInfo.InvariantCulture),

            [typeof(byte)] = s => ParseNumber<byte>(s, byte.Parse, byte.TryParse),
            [typeof(char)] = s => ParseChar(s),

            [typeof(bool)] = s => ParseBool(s),

            [typeof(DateTime)] = s => DateTime.Parse(s, CultureInfo.InvariantCulture),
            [typeof(DateTimeOffset)] = s => DateTimeOffset.Parse(s, CultureInfo.InvariantCulture),
            [typeof(TimeSpan)] = s => TimeSpan.Parse(s, CultureInfo.InvariantCulture),

            [typeof(Uri)] = s => new Uri(s),

            [typeof(FileInfo)] = s => new FileInfo(s),
            [typeof(DirectoryInfo)] = s => new DirectoryInfo(s)
        };

        private delegate T ParseNumberFunc<out T>(string input);

        private delegate bool TryParseNumberFunc<T>(string input, NumberStyles style, IFormatProvider provider, out T result);

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

        private static T ParseNumber<T>(string input, ParseNumberFunc<T> parse, TryParseNumberFunc<T> tryParseWithFormat)
        {
            if (input.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) &&
                tryParseWithFormat(input.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result))
            {
                return result;
            }

            return parse(input);
        }

        private static char ParseChar(string input)
        {
            if (input.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase) &&
                byte.TryParse(input.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result))
            {
                return (char)result;
            }

            return char.Parse(input);
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
                return TryGetParser(wrappedType, out parser);
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
    }
}
