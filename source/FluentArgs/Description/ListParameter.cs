namespace FluentArgs.Description
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using FluentArgs.Validation;

    internal class ListParameter
    {
        public static readonly IImmutableSet<string> DefaultSeparators = new[] { ";", "," }.ToImmutableHashSet();

        public ListParameter(Name name, Type type)
        {
            Name = name;
            Type = type;
            Examples = Array.Empty<string>();
            HasDefaultValue = false;
            IsRequired = false;
            Separators = DefaultSeparators;
        }

        public Name Name { get; }

        public string? Description { get; set; }

        public Type Type { get; }

        public IValidation? Validation { get; set; }

        public IReadOnlyCollection<string> Examples { get; set; }

        public object? DefaultValue { get; set; }

        public bool HasDefaultValue { get; set; }

        public bool IsRequired { get; set; }

        public IImmutableSet<string> Separators { get; set; }

        public Func<string, object>? Parser { get; set; }
    }
}
