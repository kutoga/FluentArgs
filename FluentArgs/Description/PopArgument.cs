namespace FluentArgs.Description
{
    using System;

    internal class PopArgument
    {
        public PopArgument(Type type)
        {
            Type = type;
            HasDefaultValue = false;
            Examples = Array.Empty<string>();
            IsRequired = false;
        }

        public string? Description { get; set; }

        public Type Type { get; }

        public string[] Examples { get; set; }

        public object? DefaultValue { get; set; }

        public bool HasDefaultValue { get; set; }

        public bool IsRequired { get; set; }

        public Func<string, object>? Parser { get; set; }
    }
}
