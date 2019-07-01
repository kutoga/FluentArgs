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

        public override Task Execute(State state)
        {
            var possibleParameterIndex = state.Arguments
                .Select((a, i) => (argument: a, index: i))
                .Where(p => parameter.Name.Names.Contains(p.argument))
                .Select(p => (int?)p.index)
                .FirstOrDefault();

            if (possibleParameterIndex == null)
            {
                return Next.Execute(state);
            }
            else
            {
                var parameterIndex = possibleParameterIndex.Value;
                if (parameterIndex == state.Arguments.Count - 1)
                {
                    throw new Exception("TODO");
                }

                if (!parameter.RequireExactValue)
                {
                    return thenStep.ParseFromState(state);
                }
                else
                {
                    var parameterValue = state.Arguments[parameterIndex + 1];
                    state = state.RemoveArguments(parameterIndex, parameterIndex + 1);

                    if (object.Equals(Parse(parameterValue), parameter.RequiredValue))
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

            throw new Exception("TODO: IMPLEMENT MORE DEFAULTS");
        }
    }
}
