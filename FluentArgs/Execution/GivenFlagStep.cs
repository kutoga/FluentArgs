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
            var flagIndex = state.Arguments
                .Select((a, i) => (argument: a, index: i))
                .Where(p => flag.Name.Names.Contains(p.argument))
                .Select(p => (int?)p.index)
                .FirstOrDefault();

            if (flagIndex == null)
            {
                return Next.Execute(state);
            }
            else
            {
                state = state.RemoveArguments(flagIndex.Value);
                return thenStep.ParseFromState(state);
            }
        }
    }
}
