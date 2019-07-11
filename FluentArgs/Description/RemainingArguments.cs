namespace FluentArgs.Description
{
    using System;

    internal class RemainingArguments
    {
        public RemainingArguments(Type type)
        {
            Type = type;
        }

        public string? Description { get; set; }

        public Type Type { get; }

        public Func<string, object>? Parser { get; set; }
    }
}
