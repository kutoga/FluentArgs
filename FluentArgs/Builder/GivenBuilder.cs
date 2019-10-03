namespace FluentArgs.Builder
{
    using System;
    using FluentArgs.Description;
    using FluentArgs.Execution;

    internal class GivenBuilder<TArgsBuilder> : IGiven<TArgsBuilder>
        where TArgsBuilder : class
    {
        private readonly Func<TArgsBuilder> argsBuilderFactory;
        private readonly Step previousStep;
        private readonly Func<Step, TArgsBuilder> stepWrapper;

        public GivenBuilder(Func<TArgsBuilder> argsBuilderFactory, Step previous, Func<Step, TArgsBuilder> stepWrapper)
        {
            this.argsBuilderFactory = argsBuilderFactory;
            this.previousStep = previous;
            this.stepWrapper = stepWrapper;
        }

        public IGivenCommandInitial<TArgsBuilder> Command(string name, params string[] moreNames)
        {
            return new GivenCommandBuilder<TArgsBuilder>(
                Name.ValidateAndBuild(name, moreNames),
                argsBuilderFactory,
                previousStep,
                stepWrapper);
        }

        public IGivenThen<TArgsBuilder, TArgsBuilder> Flag(string name, params string[] moreNames)
        {
            TArgsBuilder? result = default;
            return new GivenThenBuilder<TArgsBuilder, TArgsBuilder>(
                ThenExpressionBuilt,
                argsBuilderFactory(),
                () => result);

            void ThenExpressionBuilt(IParsable parsable)
            {
                result = stepWrapper(new GivenFlagStep(
                    previousStep,
                    new Flag(Name.ValidateAndBuild(name, moreNames)),
                    parsable as IParsableFromState ?? throw new Exception("TODO")));
            }
        }

        public IGivenParameter<TArgsBuilder> Parameter(string name, params string[] moreNames)
        {
            return new GivenParameterBuilder<TArgsBuilder>(Name.ValidateAndBuild(name, moreNames), argsBuilderFactory(), previousStep, stepWrapper);
        }
    }
}
