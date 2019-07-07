namespace FluentArgs.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Execution;

    internal class StepBuilder : IFluentArgsBuilder, IParsable
    {
        public Step Step { get; set; } = new InitialStep();

        //TODO: public und interface prefix weg
        IGiven<IFluentArgsBuilder> IGivenAppliable<IFluentArgsBuilder>.Given =>
                new GivenBuilder<IFluentArgsBuilder>(() => new StepBuilder(), Step, s => new StepBuilder() { Step = s });

        public IParsable Call(Action callback)
        {
            var finalStep = new CallStep(Step, new TargetFunction(callback));
            return new StepCaller(finalStep);
        }

        public IParsable Call(Func<Task> callback)
        {
            var finalStep = new CallStep(Step, new TargetFunction(callback));
            return new StepCaller(finalStep);
        }

        public IParsable Invalid()
        {
            var finalStep = new InvalidStep(Step);
            return new StepCaller(finalStep);
        }

        public void Parse(string[] args)
        {
            throw new NotImplementedException();
        }

        public Task ParseAsync(string[] args)
        {
            throw new NotImplementedException();
        }

        IConfigurableFlagWithOptionalDescription IFluentArgsBuilder.Flag(string name, params string[] moreNames)
        {
            return new FlagBuilder(s => new StepBuilder<Action<bool>, Func<bool, Task>, bool>() { Step = s }, Step, new Flag(new Name(name, moreNames)));
        }

        IConfigurableParameter<IFluentArgsBuilder<Action<TNextParam>, Func<TNextParam, Task>, TNextParam>, TNextParam> IFluentArgsBuilder.Parameter<TNextParam>(string name, params string[] moreNames)
        {
            var nextBuilder = new StepBuilder<Action<TNextParam>, Func<TNextParam, Task>, TNextParam>();
            return new ParameterBuilder<IFluentArgsBuilder<Action<TNextParam>, Func<TNextParam, Task>, TNextParam>, TNextParam>(
                ParameterBuilt, nextBuilder, new Name(name, moreNames));

            void ParameterBuilt(Parameter parameter) =>
                nextBuilder.Step = new ParameterStep(Step, parameter);
        }

        IConfigurableParameterList<IFluentArgsBuilder<Action<IReadOnlyList<TParam1>>, Func<IReadOnlyList<TParam1>, Task>, TParam1>, TParam1> IFluentArgsBuilder.ParameterList<TParam1>(string name, params string[] moreNames)
        {
            throw new NotImplementedException();
        }
    }

    internal class StepBuilder<TFunc, TFuncAsync, TParam> :
        IFluentArgsBuilder<TFunc, TFuncAsync, TParam>, IParsable
    {
        public Step Step { get; set; } = new InitialStep();

        public IGiven<IFluentArgsBuilder<TFunc, TFuncAsync, TParam>> Given =>
                new GivenBuilder<IFluentArgsBuilder<TFunc, TFuncAsync, TParam>>(
                    () => new StepBuilder<TFunc, TFuncAsync, TParam>(), Step, s => new StepBuilder<TFunc, TFuncAsync, TParam>() { Step = s });

        public IParsable Call(TFunc callback)
        {
            var finalStep = new CallStep(Step, new TargetFunction(callback));
            return new StepCaller(finalStep);
        }

        public IParsable Call(TFuncAsync callback)
        {
            var finalStep = new CallStep(Step, new TargetFunction(callback));
            return new StepCaller(finalStep);
        }

        public IParsable Invalid()
        {
            throw new NotImplementedException();
        }

        public IFluentArgsBuilder<TFunc, TFuncAsync, TParam> IsOptionalWithDefault(TParam defaultValue)
        {
            throw new NotImplementedException();
        }

        public void Parse(string[] args)
        {
            throw new NotImplementedException();
        }

        public Task ParseAsync(string[] args)
        {
            throw new NotImplementedException();
        }

        public IConfigurableParameterList<IFluentArgsBuilder<TFunc, TFuncAsync, TParam>, TParam> WithExamples(TParam example, params TParam[] moreExamples)
        {
            throw new NotImplementedException();
        }

        public IConfigurableParameterList<IFluentArgsBuilder<TFunc, TFuncAsync, TParam>, TParam> WithParser(Func<string, TParam> parser)
        {
            throw new NotImplementedException();
        }

        IConfigurableFlagWithOptionalDescription<TFunc, TFuncAsync> IFluentArgsBuilder<TFunc, TFuncAsync, TParam>.Flag(string name, params string[] moreNames)
        {
            return new FlagBuilder<TFunc, TFuncAsync, TParam>(s => new StepBuilder<Func<bool, TFunc>, Func<bool, TFuncAsync>, bool>() { Step = s }, Step, new Flag(new Name(name, moreNames)));
        }

        IConfigurableParameter<IFluentArgsBuilder<Func<TNextParam, TFunc>, Func<TNextParam, TFuncAsync>, TNextParam>, TNextParam> IFluentArgsBuilder<TFunc, TFuncAsync, TParam>.Parameter<TNextParam>(string name, params string[] moreNames)
        {
            var nextBuilder = new StepBuilder<Func<TNextParam, TFunc>, Func<TNextParam, TFuncAsync>, TNextParam>();
            return new ParameterBuilder<IFluentArgsBuilder<Func<TNextParam, TFunc>, Func<TNextParam, TFuncAsync>, TNextParam>, TNextParam>(
                ParameterBuilt, nextBuilder, new Name(name, moreNames));

            void ParameterBuilt(Parameter parameter) =>
                nextBuilder.Step = new ParameterStep(Step, parameter);
        }

        IConfigurableParameterList<IFluentArgsBuilder<Func<IReadOnlyList<TNextParam>, TFunc>, Func<IReadOnlyList<TNextParam>, TFuncAsync>, TNextParam>, TNextParam> IFluentArgsBuilder<TFunc, TFuncAsync, TParam>.ParameterList<TNextParam>(string name, params string[] moreNames)
        {
            throw new NotImplementedException();
        }
    }
}
