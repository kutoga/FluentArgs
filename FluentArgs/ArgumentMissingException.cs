using System;
using FluentArgs.Description;

namespace FluentArgs
{
    internal class ArgumentMissingException : Exception
    {
        public ArgumentMissingException(string description, Name? argumentName)
        {
            Description = description;
            ArgumentName = argumentName;
        }

        public string Description { get; }

        public Name? ArgumentName { get; }
    }
}