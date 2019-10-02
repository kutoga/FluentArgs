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
            if (state.TryExtractNamedArgument(Description.Name.Names, out var argument, out var value, out var newState))
            {
                state = newState.AddParameter(Parse(value));
            }
            else
            {
                if (Description.IsRequired)
                {
                    throw new ArgumentMissingException("Required (list-)parameter not found!", Description.Name);
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
                return ArgumentParsingException.ParseWrapper(() => ParseWithParser(parser), Description.Name);
            }

            throw ArgumentParsingException.NoParserFound(Description.Name);

            object ParseWithParser(Func<string, object?> parser)
            {
                return Reflection.Array.Create(Description.Type, splitParameters.Select(p => parser(p))
                    .ValidateIfRequired(Description.Validator, Description.Name).ToArray());
            }
        }
    }
}
