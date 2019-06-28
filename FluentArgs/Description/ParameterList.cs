namespace FluentArgs.Description
{
    using System;

    internal class ParameterList
    {
        public ParameterList(Name name, Type type)
        {
            Name = name;
            Type = type;
            HasDefaultValue = false;
            IsRequired = false;
        }

        public Name Name { get; }

        public string? Description { get; set; }

        public Type Type { get; }

        public object? DefaultValue { get; set; }

        public bool HasDefaultValue { get; set; }

        public bool IsRequired { get; set; }

        public Func<string, object>? Parser { get; set; }
    }
}
