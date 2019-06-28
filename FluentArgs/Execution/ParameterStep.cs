namespace FluentArgs.Execution
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;

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
            // TODO: Implement defaults
            var parameterIndex = state.Arguments
                .Select((a, i) => (argument: a, index: i))
                .Where(p => parameter.Name.Names.Contains(p.argument))
                .Select(p => (int?)p.index)
                .FirstOrDefault() ?? throw new Exception("TODO");

            if (parameterIndex == state.Arguments.Count)
            {
                throw new Exception("TODO");
            }

            state = state
                .AddParameter(Parse(state.Arguments[parameterIndex + 1]))
                .RemoveArguments(parameterIndex, parameterIndex + 1);

            return Next.Execute(state);
        }

        private object Parse(string parameter)
        {
            if (this.parameter.Parser != null)
            {
                return this.parameter.Parser(parameter);
            }

            if (this.parameter.Type == typeof(string))
            {
                return parameter;
            }

            throw new Exception("TODO: IMPLEMENT MORE DEFAULTS");
        }
    }
}
