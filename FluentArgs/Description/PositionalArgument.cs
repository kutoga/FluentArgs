namespace FluentArgs.Description
{
    using FluentArgs.Validation;
    using System;
    using System.Collections.Generic;

    internal class PositionalArgument
    {
        public PositionalArgument(Type type)
        {
            Type = type;
            HasDefaultValue = false;
            Examples = Array.Empty<string>();
            IsRequired = false;
        }

        public string? Description { get; set; }

        public Type Type { get; }

        public IValidator? Validator { get; set; }

        public IReadOnlyCollection<string> Examples { get; set; }

        public object? DefaultValue { get; set; }

        public bool HasDefaultValue { get; set; }

        public bool IsRequired { get; set; }

        public Func<string, object>? Parser { get; set; }
    }
}
