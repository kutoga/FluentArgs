namespace FluentArgs.Execution
{
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Parser;

    internal class GivenParameterStep : Step
    {
        public GivenParameterStep(Step previous, GivenParameter description, IParsableFromState thenStep)
            : base(previous)
        {
            Description = description;
            ThenStep = thenStep;
        }

        public GivenParameter Description { get; }

        public IParsableFromState ThenStep { get; }

        public override Task Accept(IStepVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override Task Execute(State state)
        {
            if (!state.TryExtractNamedArgument(Description.Name.Names, out _, out var value, out var newState))
            {
                return GetNextStep().Execute(state);
            }
            else
            {
                if (!Description.RequireExactValue)
                {
                    // TODO: Do we really want to work here with the old state? -> create a test
                    return ThenStep.ParseFromState(state);
                }
                else
                {
                    state = newState;

                    if (object.Equals(Parse(value!), Description.RequiredValue))
                    {
                        return ThenStep.ParseFromState(state);
                    }

                    return GetNextStep().Execute(state);
                }
            }
        }

        // TODO: Remove duplicate code (see parametersetp.cs)
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

            throw new ArgumentParsingException($"No parse for the type '{this.Description.Type.Name}' available!", this.Description.Name);
        }
    }
}
