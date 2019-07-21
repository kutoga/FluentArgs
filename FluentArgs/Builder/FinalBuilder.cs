namespace FluentArgs.Builder
{
    using FluentArgs.Execution;

    internal class FinalBuilder : IBuildable
    {
        private readonly Step step;

        public FinalBuilder(Step step)
        {
            this.step = step;
        }

        public IParsable Build()
        {
            return new FluentArgsDefinition(GetInitialStep());
        }

        private InitialStep GetInitialStep()
        {
            var step = this.step;
            while (step.Previous != null)
            {
                step = step.Previous;
            }

            return (InitialStep)step;
        }
    }
}
