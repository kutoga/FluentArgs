namespace FluentArgs.Description
{
    using System;
    using System.Collections.Immutable;

    internal class ParameterList
    {
        public static readonly IImmutableSet<string> DefaultSeparators = new[] { ";", "," }.ToImmutableHashSet();

        public ParameterList(Name name, Type type)
        {
            Name = name;
            Type = type;
            HasDefaultValue = false;
            IsRequired = false;
            Separators = DefaultSeparators;
        }

        public Name Name { get; }

        public string? Description { get; set; }

        public Type Type { get; }

        public object? DefaultValue { get; set; }

        public bool HasDefaultValue { get; set; }

        public bool IsRequired { get; set; }

        public IImmutableSet<string> Separators { get; set; }

        public Func<string, object>? Parser { get; set; }
    }
}
