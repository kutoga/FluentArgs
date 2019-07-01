namespace FluentArgs.Builder
{
    using System;
    using FluentArgs.Description;
    using FluentArgs.Execution;

    internal class GivenBuilder<TArgsBuilder> : IGiven<TArgsBuilder>
    {
        private readonly TArgsBuilder argsBuilder;
        private readonly Step previousStep;
        private readonly Func<Step, TArgsBuilder> stepWrapper;

        public GivenBuilder(TArgsBuilder argsBuilder, Step previous, Func<Step, TArgsBuilder> stepWrapper)
        {
            this.argsBuilder = argsBuilder;
            this.previousStep = previous;
            this.stepWrapper = stepWrapper;
        }

        public IGivenCommandInitial<TArgsBuilder> Command(string name, params string[] moreNames)
        {
            throw new NotImplementedException();
        }

        public IGivenThen<TArgsBuilder, TArgsBuilder> Flag(string name, params string[] moreNames)
        {
            TArgsBuilder result = default;
            return new GivenThenBuilder<TArgsBuilder, TArgsBuilder>(
                ThenExpressionBuilt,
                argsBuilder, //TODO: new builder inside the resulting givenstep
                () => result);

            void ThenExpressionBuilt(IParsable parsable)
            {
                result = stepWrapper(new GivenFlagStep(
                    previousStep,
                    new Flag(new Name(name, moreNames)),
                    parsable as IParsableFromState ?? throw new Exception("TODO")));
            }
        }

        public IGivenParameter<TArgsBuilder> Parameter(string name, params string[] moreNames)
        {
            return new GivenParameterBuilder<TArgsBuilder>(new Name(name, moreNames), argsBuilder, previousStep, stepWrapper);
        }
    }
}
