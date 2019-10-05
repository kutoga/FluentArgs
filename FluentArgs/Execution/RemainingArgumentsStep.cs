﻿namespace FluentArgs.Execution
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Parser;

    internal class RemainingArgumentsStep : Step
    {
        public RemainingArgumentsStep(Step previousStep, RemainingArguments remainingArguments)
            : base(previousStep)
        {
            Description = remainingArguments;
        }

        public RemainingArguments Description { get; }

        public override Task Accept(IStepVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override Task Execute(State state)
        {
            var remainingArguments = state.GetRemainingArguments(out state);
            var parameter = Reflection.Array.Create(this.Description.Type, remainingArguments.Select(a => Parse(a))
                .ValidateIfRequired(Description.Validation).ToArray());
            state = state.AddParameter(parameter);
            return GetNextStep().Execute(state);
        }

        private object Parse(string parameter)
        {
            if (Description.Parser != null)
            {
                return Description.Parser(parameter);
            }

            if (DefaultStringParsers.TryGetParser(Description.Type, out var parser))
            {
                return parser!(parameter);
            }

            throw new ArgumentParsingException($"No parse for the type '{this.Description.Type.Name}' available!");
        }
    }
}
