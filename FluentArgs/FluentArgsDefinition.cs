namespace FluentArgs
{
    using System.Threading.Tasks;
    using FluentArgs.Execution;

    internal class FluentArgsDefinition : IParsableFromState
    {
        private readonly InitialStep initialStep;

        public FluentArgsDefinition(InitialStep initialStep)
        {
            this.initialStep = initialStep;
        }

        public void Parse(params string[] args)
        {
            ParseAsync(args).Wait();
        }

        public Task ParseAsync(params string[] args)
        {
            return ParseFromState(State.InitialState(args));
        }

        public Task ParseFromState(State state)
        {
            return initialStep.Execute(state);
        }
    }
}
