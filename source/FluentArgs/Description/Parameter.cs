namespace FluentArgs.Description
{
    using System;
    using System.Collections.Generic;
    using FluentArgs.Validation;

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

        public IValidation? Validation { get; set; }

        public IReadOnlyCollection<string> Examples { get; set; }

        public object? DefaultValue { get; set; }

        public bool HasDefaultValue { get; set; }

        public bool IsRequired { get; set; }

        public Func<string, object>? Parser { get; set; }
    }
}
