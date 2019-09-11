using FluentArgs.Description;
using FluentArgs.Help;

namespace FluentArgs.Builder
{
    using FluentArgs.Execution;

    internal class FinalBuilder : IBuildable
    {
        private readonly Step step;
        private readonly ILineWriter errorLineWriter;

        public FinalBuilder(Step step, ILineWriter errorLineWriter)
        {
            this.step = step;
            this.errorLineWriter = errorLineWriter;
        }

        public IParsable Build()
        {
            return new FluentArgsDefinition(GetInitialStep(), errorLineWriter);
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
