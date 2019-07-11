namespace FluentArgs.Description
{
    using System;

    internal class RemainingArguments
    {
        public RemainingArguments(string description, Type type, Func<string, object> parser)
        {
            Description = description;
            Type = type;
            Parser = parser;
        }

        public string? Description { get; set; }

        public Type Type { get; }

        public Func<string, object>? Parser { get; set; }
    }
}
