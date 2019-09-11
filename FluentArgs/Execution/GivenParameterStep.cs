namespace FluentArgs.Execution
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Parser;

    internal class GivenParameterStep : Step
    {
        private readonly GivenParameter parameter;
        private readonly IParsableFromState thenStep;

        public GivenParameterStep(Step previous, GivenParameter parameter, IParsableFromState thenStep)
            : base(previous)
        {
            this.parameter = parameter;
            this.thenStep = thenStep;
        }

        public override Task Accept(IStepVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override Task Execute(State state)
        {
            if (!state.TryExtractArguments(parameter.Name.Names, out var arguments, out var newState, 1))
            {
                return Next.Execute(state);
            }
            else
            {
                if (!parameter.RequireExactValue)
                {
                    //TODO: Do we really want to work here with the old state? -> create a test
                    return thenStep.ParseFromState(state);
                }
                else
                {
                    state = newState;

                    if (object.Equals(Parse(arguments[1]), parameter.RequiredValue))
                    {
                        return thenStep.ParseFromState(state);
                    }

                    return Next.Execute(state);
                }
            }
        }

        //TODO: Remove duplicate code (see parametersetp.cs)
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

            throw new ArgumentParsingException($"No parse for the type '{this.parameter.Type.Name}' available!", this.parameter.Name);
        }
    }
}
