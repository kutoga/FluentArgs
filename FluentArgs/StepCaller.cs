namespace FluentArgs
{
    using System.Threading.Tasks;
    using FluentArgs.Execution;

    internal class StepCaller : IParsableFromState
    {
        private readonly Step step;

        public StepCaller(Step step)
        {
            this.step = step;
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

        public void Parse(string[] args)
        {
            ParseAsync(args).Wait();
        }

        public Task ParseAsync(string[] args)
        {
            return ParseFromState(State.InitialState(args));
        }

        public Task ParseFromState(State state)
        {
            var initialStep = GetInitialStep();
            return initialStep.Execute(state);
        }
    }
}
