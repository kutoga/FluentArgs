namespace FluentArgs.Execution
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Parser;
    using FluentArgs.Reflection;

    internal class ParameterStep : Step
    {
        public Parameter Description { get; }

        public ParameterStep(Step previous, Parameter parameter)
            : base(previous)
        {
            this.Description = parameter;
        }

        public override Task Accept(IStepVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override Task Execute(State state)
        {
            if (state.TryExtractArguments(Description.Name.Names, out var arguments, out var newState, 1))
            {
                state = newState.AddParameter(Parse(arguments[1]));
            }
            else
            {
                if (Description.IsRequired)
                {
                    throw new Exception("TODO: parameter is required, but not given");
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
