namespace FluentArgs
{
    using System;
    using FluentArgs.Description;

    internal class ArgumentMissingException : Exception
    {
        public ArgumentMissingException(string description, Type type, Name? argumentName = null)
        {
            Description = description;
            Type = type;
            ArgumentName = argumentName;
        }

        public string Description { get; }

        public Name? ArgumentName { get; }

        public Type Type { get; }
    }
}