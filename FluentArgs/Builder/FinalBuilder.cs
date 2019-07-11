using FluentArgs.Execution;

namespace FluentArgs.Builder
{
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

        private Step GetInitialStep()
        {
            var step = this.step;
            while (step.Previous != null)
            {
                step = step.Previous;
            }

            return step;
        }
    }
}
