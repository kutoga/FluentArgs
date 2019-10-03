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
        public ParameterStep(Step previous, Parameter parameter)
            : base(previous)
        {
            this.Description = parameter;
        }

        public Parameter Description { get; }

        public override Task Accept(IStepVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override Task Execute(State state)
        {
            if (state.TryExtractNamedArgument(Description.Name.Names, out _, out var value, out var newState))
            {
                state = newState.AddParameter(Parse(value!).ValidateIfRequired(Description.Validator, Description.Name));
            }
            else
            {
                if (Description.IsRequired)
                {
                    throw new ArgumentMissingException("Required parameter not found!", Description.Name);
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

        private object Parse(string parameter)
        {
            if (this.Description.Parser != null)
            {
                return this.Description.Parser(parameter);
            }

            if (DefaultStringParsers.TryGetParser(this.Description.Type, out var parser))
            {
                return ArgumentParsingException.ParseWrapper(() => parser!(parameter), Description.Name);
            }

            throw ArgumentParsingException.NoParserFound(Description.Name);
        }
    }
}
