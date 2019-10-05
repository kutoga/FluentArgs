namespace FluentArgs
{
    using System;
    using FluentArgs.Description;

    internal class ArgumentMissingException : Exception
    {
        public ArgumentMissingException(string description, Name? argumentName = null)
        {
            Description = description;
            ArgumentName = argumentName;
        }

        public string Description { get; }

        public Name? ArgumentName { get; }
    }
}