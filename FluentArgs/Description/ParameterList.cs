namespace FluentArgs.Description
{
    using FluentArgs.Validation;
    using System;
    using System.Collections.Immutable;

    internal class ParameterList
    {
        public static readonly IImmutableSet<string> DefaultSeparators = new[] { ";", "," }.ToImmutableHashSet();

        public ParameterList(Name name, Type type)
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

        public IValidator? Validator { get; set; }

        public string[] Examples { get; set; }

        public object? DefaultValue { get; set; }

        public bool HasDefaultValue { get; set; }

        public bool IsRequired { get; set; }

        public IImmutableSet<string> Separators { get; set; }

        public Func<string, object>? Parser { get; set; }
    }
}
