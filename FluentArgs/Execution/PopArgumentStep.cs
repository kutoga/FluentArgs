namespace FluentArgs.Execution
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Parser;
    using FluentArgs.Reflection;

    internal class PopArgumentStep : Step
    {
        public PopArgument Description { get; }

        public PopArgumentStep(Step previous, PopArgument popArgument)
            : base(previous)
        {
            this.Description = popArgument;
        }

        public override Task Accept(IStepVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override Task Execute(State state)
        {
            if (/*TODO*/ false)
            {
                //state = newState.AddParameter(Parse(arguments[1]));
            }
            else
            {
                if (Description.IsRequired)
                {
                    throw new ArgumentMissingException("Required popArgument not found!", Description.Name);
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
