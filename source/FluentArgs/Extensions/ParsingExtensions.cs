namespace FluentArgs.Extensions
{
    using System;
    using FluentArgs.Description;
    using FluentArgs.Parser;

    internal static class ParsingExtensions
    {
        public static object TryParse(this string input, Type targetType, Func<string, object>? preferredParser, Name? argumentName = null)
        {
            var parser = preferredParser ?? GetDefaultParser();
            return ArgumentParsingException.ParseWrapper(() => parser(input), targetType, argumentName);

            Func<string, object> GetDefaultParser()
            {
                if (!DefaultStringParsers.TryGetParser(targetType, out var parser))
                {
                    throw ArgumentParsingException.NoParserFound(targetType, argumentName);
                }

                return parser!;
            }
        }
    }
}
