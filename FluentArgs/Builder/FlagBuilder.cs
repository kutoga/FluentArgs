namespace FluentArgs.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Execution;

    internal class FlagBuilder : IConfigurableFlagWithOptionalDescription
    {
        private readonly Func<Step, IFluentArgsBuilder<Action<bool>, Func<bool, Task>>> stepWrapper;
        private readonly Step previousStep;
        private readonly Flag flag;

        public FlagBuilder(Func<Step, IFluentArgsBuilder<Action<bool>, Func<bool, Task>>> stepWrapper, Step previousStep, Flag flag)
        {
            this.stepWrapper = stepWrapper;
            this.previousStep = previousStep;
            this.flag = flag;
        }

        public IGiven<IFluentArgsBuilder<Action<bool>, Func<bool, Task>>> Given => Build().Given;

        public IParsable Call(Action<bool> callback)
        {
            return Build().Call(callback);
        }

        public IParsable Call(Func<bool, Task> callback)
        {
            return Build().Call(callback);
        }

        public IParsable Call(Func<IReadOnlyList<string>, Action<bool>> callbackWithAdditionalArgs)
        {
            return Build().Call(callbackWithAdditionalArgs);
        }

        public IParsable Call(Func<IReadOnlyList<string>, Func<bool, Task>> callbackWithAdditionalArgs)
        {
            return Build().Call(callbackWithAdditionalArgs);
        }

        public IConfigurableFlagWithOptionalDescription<Action<bool>, Func<bool, Task>> Flag(string name, params string[] moreNames)
        {
            return Build().Flag(name, moreNames);
        }

        public IParsable Invalid()
        {
            return Build().Invalid();
        }

        public IConfigurableParameter<IFluentArgsBuilder<Func<TNextParam, Action<bool>>, Func<TNextParam, Func<bool, Task>>>, TNextParam> Parameter<TNextParam>(string name, params string[] moreNames)
        {
            return Build().Parameter<TNextParam>(name, moreNames);
        }

        public IConfigurableParameterList<IFluentArgsBuilder<Func<IReadOnlyList<TNextParam>, Action<bool>>, Func<IReadOnlyList<TNextParam>, Func<bool, Task>>>, TNextParam> ParameterList<TNextParam>(string name, params string[] moreNames)
        {
            return Build().ParameterList<TNextParam>(name, moreNames);
        }

        public IFluentArgsBuilder<Action<bool>, Func<bool, Task>> WithDescription(string description)
        {
            flag.Description = description;
            return Build();
        }

        private IFluentArgsBuilder<Action<bool>, Func<bool, Task>> Build()
        {
            return stepWrapper(new FlagStep(previousStep, flag));
        }
    }

    internal class FlagBuilder<TFunc, TFuncAsync> : IConfigurableFlagWithOptionalDescription<TFunc, TFuncAsync>
    {
        private readonly Func<Step, IFluentArgsBuilder<Func<bool, TFunc>, Func<bool, TFuncAsync>>> stepWrapper;
        private readonly Step previousStep;
        private readonly Flag flag;

        public FlagBuilder(Func<Step, IFluentArgsBuilder<Func<bool, TFunc>, Func<bool, TFuncAsync>>> stepWrapper, Step previousStep, Flag flag)
        {
            this.stepWrapper = stepWrapper;
            this.previousStep = previousStep;
            this.flag = flag;
        }

        public IGiven<IFluentArgsBuilder<Func<bool, TFunc>, Func<bool, TFuncAsync>>> Given => Build().Given;

        public IParsable Call(Func<bool, TFunc> callback)
        {
            return Build().Call(callback);
        }

        public IParsable Call(Func<bool, TFuncAsync> callback)
        {
            return Build().Call(callback);
        }

        public IParsable Call(Func<IReadOnlyList<string>, Func<bool, TFunc>> callbackWithAdditionalArgs)
        {
            return Build().Call(callbackWithAdditionalArgs);
        }

        public IParsable Call(Func<IReadOnlyList<string>, Func<bool, TFuncAsync>> callbackWithAdditionalArgs)
        {
            return Build().Call(callbackWithAdditionalArgs);
        }

        public IConfigurableFlagWithOptionalDescription<Func<bool, TFunc>, Func<bool, TFuncAsync>> Flag(string name, params string[] moreNames)
        {
            return Build().Flag(name, moreNames);
        }

        public IParsable Invalid()
        {
            return Build().Invalid();
        }

        public IConfigurableParameter<IFluentArgsBuilder<Func<TNextParam, Func<bool, TFunc>>, Func<TNextParam, Func<bool, TFuncAsync>>>, TNextParam> Parameter<TNextParam>(string name, params string[] moreNames)
        {
            return Build().Parameter<TNextParam>(name, moreNames);
        }

        public IConfigurableParameterList<IFluentArgsBuilder<Func<IReadOnlyList<TNextParam>, Func<bool, TFunc>>, Func<IReadOnlyList<TNextParam>, Func<bool, TFuncAsync>>>, TNextParam> ParameterList<TNextParam>(string name, params string[] moreNames)
        {
            return Build().ParameterList<TNextParam>(name, moreNames);
        }

        public IFluentArgsBuilder<Func<bool, TFunc>, Func<bool, TFuncAsync>> WithDescription(string description)
        {
            flag.Description = description;
            return Build();
        }

        private IFluentArgsBuilder<Func<bool, TFunc>, Func<bool, TFuncAsync>> Build()
        {
            return stepWrapper(new FlagStep(previousStep, flag));
        }
    }
}
