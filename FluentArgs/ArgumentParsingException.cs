using System;
using FluentArgs.Description;

namespace FluentArgs
{
    internal class ArgumentParsingException : Exception
    {
        public ArgumentParsingException(string description, Name? argumentName = null)
        {
            Description = description;
            ArgumentName = argumentName;
        }

        public string Description { get; }

        public Name? ArgumentName { get; }

        public static ArgumentParsingException NoParserFound(Name? argumentName = null)
        {
            return new ArgumentParsingException("Could not find a suitable parser!", argumentName);
        }

        public static T ParseWrapper<T>(Func<T> parser, Name? argumentName = null)
        {
            try
            {
                return parser();
            }
            catch (Exception ex)
            {
                throw new ArgumentParsingException(ex.Message, argumentName);
            }
        }
    }
}