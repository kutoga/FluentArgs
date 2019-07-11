﻿namespace FluentArgs
{
    using System.Threading.Tasks;
    using FluentArgs.Execution;

    internal class FluentArgsDefinition : IParsableFromState
    {
        private readonly Step initialStep;

        public FluentArgsDefinition(Step initialStep)
        {
            this.initialStep = initialStep;
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
            return initialStep.Execute(state);
        }
    }
}
