namespace FluentArgs.Execution
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Extensions;
    using FluentArgs.Parser;
    using FluentArgs.Reflection;

    internal class PositionalArgumentStep : Step
    {
        public PositionalArgumentStep(Step previous, PositionalArgument positionalArgument)
            : base(previous)
        {
            this.Description = positionalArgument;
        }

        public PositionalArgument Description { get; }

        public override Task Accept(IStepVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override Task Execute(State state)
        {
            if (state.PopArgument(out var argument, out var newState))
            {
                state = newState.AddParameter(
                    argument!.TryParse(Description.Type, Description.Parser)
                        .ValidateIfRequired(Description.Validation));
            }
            else
            {
                if (Description.IsRequired)
                {
                    throw new ArgumentMissingException($"Required positionalArgument not found! Argument description: {Description.Description}", Description.Type);
                }

                if (Description.HasDefaultValue)
                {
                    state = state.AddParameter(Description.DefaultValue);
                }
                else
                {
                    state = state.AddParameter(Default.Instance(Description.Type));
                }
            }

            return GetNextStep().Execute(state);
        }
    }
}
