namespace FluentArgs.Builder
{
    using System;
    using FluentArgs.Description;
    using FluentArgs.Execution;

    internal class GivenParameterBuilder<TArgsBuilder> :
        IGivenParameter<TArgsBuilder>
        where TArgsBuilder : class
    {
        private readonly Name name;
        private readonly TArgsBuilder argsBuilder;
        private readonly Step previousStep;
        private readonly Func<Step, TArgsBuilder> stepWrapper;

        public GivenParameterBuilder(Name name, TArgsBuilder argsBuilder, Step previousStep, Func<Step, TArgsBuilder> stepWrapper)
        {
            this.name = name;
            this.argsBuilder = argsBuilder;
            this.previousStep = previousStep;
            this.stepWrapper = stepWrapper;
        }

        public IGivenThen<TArgsBuilder, TArgsBuilder> Exists()
        {
            TArgsBuilder? result = default;
            return new GivenThenBuilder<TArgsBuilder, TArgsBuilder>(
                ThenExpressionBuilt,
                argsBuilder,
                () => result);

            void ThenExpressionBuilt(IParsable parsable)
            {
                result = stepWrapper(new GivenParameterStep(
                    previousStep,
                    GivenParameter.Exists(name),
                    parsable as IParsableFromState ?? throw new Exception("TODO")));
            }
        }

        public IGivenThen<TArgsBuilder, TArgsBuilder> HasValue<TParam>(TParam value, Func<string, TParam>? parser = null)
        {
            TArgsBuilder? result = default;
            return new GivenThenBuilder<TArgsBuilder, TArgsBuilder>(
                ThenExpressionBuilt,
                argsBuilder,
                () => result);

            void ThenExpressionBuilt(IParsable parsable)
            {
                result = stepWrapper(new GivenParameterStep(
                    previousStep,
                    GivenParameter.HasValue(name, typeof(TParam), value!, GetParser()),
                    parsable as IParsableFromState ?? throw new Exception("TODO")));
            }

            Func<string, object>? GetParser()
            {
                if (parser == null)
                {
                    return null;
                }

                return s => parser!(s) !;
            }
        }
    }
}
