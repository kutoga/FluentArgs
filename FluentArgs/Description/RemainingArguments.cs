namespace FluentArgs.Description
{
    using System;
    using System.Collections.Generic;
    using FluentArgs.Validation;

    internal class RemainingArguments
    {
        public RemainingArguments(Type type)
        {
            Type = type;
        }

        public string? Description { get; set; }

        public Type Type { get; }

        public IReadOnlyCollection<string>? Examples { get; set; }

        public IValidation? Validation { get; set; }

        public Func<string, object>? Parser { get; set; }
    }
}
