namespace FluentArgs
{
    using System.Threading.Tasks;
    using FluentArgs.Execution;

    internal class FluentArgsDefinition : IParsableFromState
    {
        public InitialStep InitialStep { get; }

        public FluentArgsDefinition(InitialStep initialStep)
        {
            this.InitialStep = initialStep;
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
            return InitialStep.Execute(state);
        }
    }
}
