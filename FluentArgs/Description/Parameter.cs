﻿namespace FluentArgs.Description
{
    using FluentArgs.Validation;
    using System;

    internal class Parameter
    {
        public Parameter(Name name, Type type)
        {
            Name = name;
            Type = type;
            HasDefaultValue = false;
            Examples = Array.Empty<string>();
            IsRequired = false;
        }

        public Name Name { get; }

        public string? Description { get; set; }

        public Type Type { get; }

        public IValidator? Validator { get; set; }

        public string[] Examples { get; set; }

        public object? DefaultValue { get; set; }

        public bool HasDefaultValue { get; set; }

        public bool IsRequired { get; set; }

        public Func<string, object>? Parser { get; set; }
    }
}
