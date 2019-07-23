namespace FluentArgs.Execution
{
    using System.Threading.Tasks;
    using FluentArgs.Description;

    internal class FlagStep : Step
    {
        private readonly Flag flag;

        public FlagStep(Step previousStep, Flag flag)
            : base(previousStep)
        {
            this.flag = flag;
        }

        public override Task Accept(IStepVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override Task Execute(State state)
        {
            if (state.TryExtractArguments(flag.Name.Names, out var arguments, out var newState))
            {
                state = newState.AddParameter(true);
            }
            else
            {
                state = state.AddParameter(false);
            }

            return Next.Execute(state);
        }
    }
}
