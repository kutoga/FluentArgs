namespace FluentArgs.Execution
{
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;

    internal class GivenFlagStep : Step
    {
        private readonly Flag flag;
        private readonly IParsableFromState thenStep;

        public GivenFlagStep(Step previous, Flag flag, IParsableFromState thenStep)
            : base(previous)
        {
            this.flag = flag;
            this.thenStep = thenStep;
        }

        public override Task Execute(State state)
        {
            if (!state.TryExtractArguments(flag.Name.Names, out var _, out var newState))
            {
                return Next.Execute(state);
            }
            else
            {
                return thenStep.ParseFromState(newState);
            }
        }
    }
}
