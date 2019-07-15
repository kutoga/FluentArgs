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
        private Parameter parameter;

        public ParameterStep(Step previous, Parameter parameter)
            : base(previous)
        {
            this.parameter = parameter;
        }

        public override Task Execute(State state)
        {
            if (state.TryExtractArguments(parameter.Name.Names, out var arguments, out var newState, 1))
            {
                state = newState.AddParameter(Parse(arguments[1]));
            }
            else
            {
                if (parameter.IsRequired)
                {
                    throw new Exception("TODO: parameter is required, but not given");
                }

                if (parameter.HasDefaultValue)
                {
                    state = state.AddParameter(parameter.DefaultValue);
                }
                else
                {
                    state = state.AddParameter(Default.Instance(parameter.Type));
                }
            }

            return Next.Execute(state);
        }

        private object Parse(string parameter)
        {
            if (this.parameter.Parser != null)
            {
                return this.parameter.Parser(parameter);
            }

            if (DefaultStringParsers.TryGetParser(this.parameter.Type, out var parser))
            {
                return parser!(parameter);
            }

            throw new Exception("TODO: IMPLEMENT MORE DEFAULTS");
        }
    }
}
