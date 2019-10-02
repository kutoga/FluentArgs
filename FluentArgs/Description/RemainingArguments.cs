namespace FluentArgs.Description
{
    using FluentArgs.Validation;
    using System;

    internal class RemainingArguments
    {
        public RemainingArguments(Type type)
        {
            Type = type;
        }

        public string? Description { get; set; }

        public Type Type { get; }

        public IValidator Validator { get; set; }

        public Func<string, object>? Parser { get; set; }
    }
}
