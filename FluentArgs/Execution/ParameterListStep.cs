namespace FluentArgs.Execution
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Parser;

    internal class ParameterListStep : Step
    {
        public ParameterList Description { get; } //TODO: rename to "description" (everywhere)

        public ParameterListStep(Step previous, ParameterList parameterList)
            : base(previous)
        {
            this.Description = parameterList;
        }

        public override Task Accept(IStepVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override Task Execute(State state)
        {
            if (!state.TryExtractArguments(Description.Name.Names, out var arguments, out var newState, 1))
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
                    state = state.AddParameter(null);
                }
            }
            else
            {
                state = newState.AddParameter(Parse(arguments[1]));
            }

            return Next.Execute(state);
        }

        private object Parse(string parameter)
        {
            var splitParameters = parameter.Split(Description.Separators.ToArray(), StringSplitOptions.None);

            if (Description.Parser != null)
            {
                return ParseWithParser(Description.Parser); //TODO: parseWithParser sounds stupid
            }

            if (DefaultStringParsers.TryGetParser(Description.Type, out var parser))
            {
                return ParseWithParser(parser);
            }

            throw new Exception("TODO: IMPLEMENT MORE DEFAULTS");

            object ParseWithParser(Func<string, object?> parser)
            {
                return Reflection.Array.Create(Description.Type, splitParameters.Select(p => parser(p)).ToArray());
            }
        }
    }
}
