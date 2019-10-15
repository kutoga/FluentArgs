namespace FluentArgs.Execution
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Extensions;
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
            var parameter = Reflection.Array.Create(Description.Type, remainingArguments
                .Select(a => a.TryParse(Description.Type, Description.Parser))
                .ValidateIfRequired(Description.Validation).ToArray());
            state = state.AddParameter(parameter);
            return GetNextStep().Execute(state);
        }
    }
}
