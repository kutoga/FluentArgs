﻿using System;
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
    }
}