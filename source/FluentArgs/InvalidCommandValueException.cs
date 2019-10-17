namespace FluentArgs
{
    using System;
    using FluentArgs.Description;

    internal class InvalidCommandValueException : Exception
    {
        public InvalidCommandValueException(Name commandName, string value)
        {
            CommandName = commandName;
            Value = value;
        }

        public Name CommandName { get; }

        public string Value { get; }
    }
}
