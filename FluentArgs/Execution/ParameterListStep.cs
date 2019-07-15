namespace FluentArgs.Execution
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Parser;

    internal class ParameterListStep : Step
    {
        private ParameterList parameterList; //TODO: rename to "description" (everywhere)

        public ParameterListStep(Step previous, ParameterList parameterList)
            : base(previous)
        {
            this.parameterList = parameterList;
        }

        public override Task Execute(State state)
        {
            if (!state.TryExtractArguments(parameterList.Name.Names, out var arguments, out var newState, 1))
            {
                if (parameterList.IsRequired)
                {
                    throw new Exception("TODO: parameter is required, but not given");
                }

                if (parameterList.HasDefaultValue)
                {
                    state = state.AddParameter(parameterList.DefaultValue);
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
            var splitParameters = parameter.Split(parameterList.Separators.ToArray(), StringSplitOptions.None);

            if (parameterList.Parser != null)
            {
                return ParseWithParser(parameterList.Parser); //TODO: parseWithParser sounds stupid
            }

            if (DefaultStringParsers.TryGetParser(parameterList.Type, out var parser))
            {
                return ParseWithParser(parser);
            }

            throw new Exception("TODO: IMPLEMENT MORE DEFAULTS");

            object ParseWithParser(Func<string, object?> parser)
            {
                return Reflection.Array.Create(parameterList.Type, splitParameters.Select(p => parser(p)).ToArray());
            }
        }
    }
}
