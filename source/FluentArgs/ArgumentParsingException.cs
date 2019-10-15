namespace FluentArgs
{
    using System;
    using FluentArgs.Description;

    internal class ArgumentParsingException : Exception
    {
        public ArgumentParsingException(string description, Type targetType, Name? argumentName = null)
        {
            Description = description;
            TargetType = targetType;
            ArgumentName = argumentName;
        }

        public string Description { get; }

        public Type TargetType { get; }

        public Name? ArgumentName { get; }

        public static ArgumentParsingException NoParserFound(Type targetType, Name? argumentName = null)
        {
            return new ArgumentParsingException("Could not find a suitable parser!", targetType, argumentName);
        }

        public static object ParseWrapper(Func<object> parser, Type targetType, Name? argumentName = null)
        {
            try
            {
                return parser();
            }
            catch (Exception ex)
            {
                throw new ArgumentParsingException(ex.Message, targetType, argumentName);
            }
        }
    }
}