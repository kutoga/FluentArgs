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
        private readonly Func<Step, IFluentArgsBuilder<Action<bool>, Func<bool, Task>, bool>> stepWrapper;
        private readonly Step previousStep;
        private readonly Flag flag;

        public FlagBuilder(Func<Step, IFluentArgsBuilder<Action<bool>, Func<bool, Task>, bool>> stepWrapper, Step previousStep, Flag flag)
        {
            this.stepWrapper = stepWrapper;
            this.previousStep = previousStep;
            this.flag = flag;
        }

        public IGiven<IFluentArgsBuilder<Action<bool>, Func<bool, Task>, bool>> Given => Build().Given;

        public IParsable Call(Action<bool> callback)
        {
            return Build().Call(callback);
        }

        public IParsable Call(Func<bool, Task> callback)
        {
            return Build().Call(callback);
        }

        public IConfigurableFlagWithOptionalDescription<Action<bool>, Func<bool, Task>, bool> Flag(string name, params string[] moreNames)
        {
            return Build().Flag(name, moreNames);
        }

        public IParsable Invalid()
        {
            return Build().Invalid();
        }

        public IConfigurableParameter<IFluentArgsBuilder<Func<TNextParam, Action<bool>>, Func<TNextParam, Func<bool, Task>>, TNextParam>, TNextParam> Parameter<TNextParam>(string name, params string[] moreNames)
        {
            return Build().Parameter<TNextParam>(name, moreNames);
        }

        public IConfigurableParameter<IFluentArgsBuilder<Func<IReadOnlyList<TNextParam>, Action<bool>>, Func<IReadOnlyList<TNextParam>, Func<bool, Task>>, TNextParam>, TNextParam> ParameterList<TNextParam>(string name, params string[] moreNames)
        {
            return Build().ParameterList<TNextParam>(name, moreNames);
        }

        public IFluentArgsBuilder<Action<bool>, Func<bool, Task>, bool> WithDescription(string description)
        {
            flag.Description = description;
            return Build();
        }

        private IFluentArgsBuilder<Action<bool>, Func<bool, Task>, bool> Build()
        {
            return stepWrapper(new FlagStep(previousStep, flag));
        }
    }

    internal class FlagBuilder<TFunc, TFuncAsync, TParam> : IConfigurableFlagWithOptionalDescription<TFunc, TFuncAsync, TParam>
    {
        private readonly Func<Step, IFluentArgsBuilder<Func<bool, TFunc>, Func<bool, TFuncAsync>, bool>> stepWrapper;
        private readonly Step previousStep;
        private readonly Flag flag;

        public FlagBuilder(Func<Step, IFluentArgsBuilder<Func<bool, TFunc>, Func<bool, TFuncAsync>, bool>> stepWrapper, Step previousStep, Flag flag)
        {
            this.stepWrapper = stepWrapper;
            this.previousStep = previousStep;
            this.flag = flag;
        }

        public IGiven<IFluentArgsBuilder<Func<bool, TFunc>, Func<bool, TFuncAsync>, bool>> Given => Build().Given;

        public IParsable Call(Func<bool, TFunc> callback)
        {
            return Build().Call(callback);
        }

        public IParsable Call(Func<bool, TFuncAsync> callback)
        {
            return Build().Call(callback);
        }

        public IConfigurableFlagWithOptionalDescription<Func<bool, TFunc>, Func<bool, TFuncAsync>, bool> Flag(string name, params string[] moreNames)
        {
            return Build().Flag(name, moreNames);
        }

        public IParsable Invalid()
        {
            return Build().Invalid();
        }

        public IConfigurableParameter<IFluentArgsBuilder<Func<TNextParam, Func<bool, TFunc>>, Func<TNextParam, Func<bool, TFuncAsync>>, TNextParam>, TNextParam> Parameter<TNextParam>(string name, params string[] moreNames)
        {
            return Build().Parameter<TNextParam>(name, moreNames);
        }

        public IConfigurableParameter<IFluentArgsBuilder<Func<IReadOnlyList<TNextParam>, Func<bool, TFunc>>, Func<IReadOnlyList<TNextParam>, Func<bool, TFuncAsync>>, TNextParam>, TNextParam> ParameterList<TNextParam>(string name, params string[] moreNames)
        {
            return Build().ParameterList<TNextParam>(name, moreNames);
        }

        public IFluentArgsBuilder<Func<bool, TFunc>, Func<bool, TFuncAsync>, bool> WithDescription(string description)
        {
            flag.Description = description;
            return Build();
        }

        private IFluentArgsBuilder<Func<bool, TFunc>, Func<bool, TFuncAsync>, bool> Build()
        {
            return stepWrapper(new FlagStep(previousStep, flag));
        }
    }
}
