namespace FluentArgs.Execution
{
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;

    internal class GivenFlagStep : Step
    {
        public GivenFlagStep(Step previous, Flag description, IParsableFromState thenStep)
            : base(previous)
        {
            Description = description;
            ThenStep = thenStep;
        }

        public IParsableFromState ThenStep { get; }
        public Flag Description { get; }

        public override Task Accept(IStepVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override Task Execute(State state)
        {
            if (!state.TryExtractFlag(Description.Name.Names, out _, out var newState))
            {
                return Next.Execute(state);
            }
            else
            {
                return ThenStep.ParseFromState(newState);
            }
        }
    }
}
