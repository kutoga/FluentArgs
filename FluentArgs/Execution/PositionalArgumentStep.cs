namespace FluentArgs.Execution
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Parser;
    using FluentArgs.Reflection;

    internal class PositionalArgumentStep : Step
    {
        public PositionalArgument Description { get; }

        public PositionalArgumentStep(Step previous, PositionalArgument positionalArgument)
            : base(previous)
        {
            this.Description = positionalArgument;
        }

        public override Task Accept(IStepVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override Task Execute(State state)
        {
            if (state.PopArgument(out var argument, out var newState))
            {
                state = newState.AddParameter(Parse(argument));
            }
            else
            {
                if (Description.IsRequired)
                {
                    throw new ArgumentMissingException($"Required positionalArgument not found! Argument description: {Description.Description}");
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

            return Next.Execute(state);
        }

        private object Parse(string parameter)
        {
            if (this.Description.Parser != null)
            {
                return this.Description.Parser(parameter);
            }

            if (DefaultStringParsers.TryGetParser(this.Description.Type, out var parser))
            {
                return parser!(parameter);
            }

            throw new Exception("TODO: IMPLEMENT MORE DEFAULTS");
        }
    }
}
