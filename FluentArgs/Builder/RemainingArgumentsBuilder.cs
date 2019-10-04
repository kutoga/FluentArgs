namespace FluentArgs.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Execution;
    using FluentArgs.Validation;

    internal class RemainingArgumentsBuilder<TFunc, TFuncAsync, TParam> :
        IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam>
    {
        private readonly Func<Step, ICallable<TFunc, TFuncAsync>> stepWrapper;
        private readonly Step previousStep;
        private readonly RemainingArguments remainingArguments;

        public RemainingArgumentsBuilder(Func<Step, ICallable<TFunc, TFuncAsync>> stepWrapper, Step previousStep) // TODO: inconsistent naming previous vs previousStep
        {
            this.stepWrapper = stepWrapper;
            this.previousStep = previousStep;
            remainingArguments = new RemainingArguments(typeof(TParam));
        }

        public IBuildable Call(TFunc callback)
        {
            return Finalize().Call(callback);
        }

        public IBuildable Call(TFuncAsync callback)
        {
            return Finalize().Call(callback);
        }

        public IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam> WithDescription(string description)
        {
            remainingArguments.Description = description;
            return this;
        }

        public IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam> WithExamples(TParam example, params TParam[] moreExamples)
        {
            remainingArguments.Examples = Examples.Pack(example, moreExamples);
            return this;
        }

        public IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam> WithExamples(string example, params string[] moreExamples)
        {
            remainingArguments.Examples = Examples.Pack(example, moreExamples);
            return this;
        }

        public IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam> WithParser(Func<string, TParam> parser)
        {
            remainingArguments.Parser = s => parser(s) !;
            return this;
        }

        public IBuildable CallUntyped(Action<IReadOnlyCollection<object?>> callback)
        {
            return Finalize().CallUntyped(callback);
        }

        public IBuildable CallUntyped(Func<IReadOnlyCollection<object?>, Task> callback)
        {
            return Finalize().CallUntyped(callback);
        }

        public IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam> WithValidator(IValidator<TParam> validator)
        {
            remainingArguments.Validator = validator.ToObjectValidator();
            return this;
        }

        private ICallable<TFunc, TFuncAsync> Finalize()
        {
            var step = new RemainingArgumentsStep(previousStep, remainingArguments);
            return stepWrapper(step);
        }
    }
}
