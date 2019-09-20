namespace FluentArgs.Execution
{
    using System.Threading.Tasks;
    using FluentArgs.Description;

    internal class FlagStep : Step
    {
        public Flag Description { get; }

        public FlagStep(Step previousStep, Flag flag)
            : base(previousStep)
        {
            this.Description = flag;
        }

        public override Task Accept(IStepVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override Task Execute(State state)
        {
            if (state.TryExtractFlag(Description.Name.Names, out var _, out var newState))
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
