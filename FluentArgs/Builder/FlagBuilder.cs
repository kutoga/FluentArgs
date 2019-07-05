using FluentArgs.Description;
using FluentArgs.Execution;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentArgs.Builder
{
    internal class FlagBuilder : IConfigurableFlagWithOptionalDescription
    {
        private readonly Func<Step, IFluentArgsBuilder<Action<bool>, Func<bool, Task>, bool>> stepWrapper;
        private readonly Flag flag;

        public FlagBuilder(Func<Step, IFluentArgsBuilder<Action<bool>, Func<bool, Task>, bool>> stepWrapper, Flag flag)
        {
            this.stepWrapper = stepWrapper;
            this.flag = flag;
        }

        private IFluentArgsBuilder<Action<bool>, Func<bool, Task>, bool> Build()
        {
            return stepWrapper(new FlagSt
        }

        public IGiven<IFluentArgsBuilder> Given
        {
            get
            {

            }
        }

        public IParsable Call(Action callback)
        {
            throw new NotImplementedException();
        }

        public IParsable Call(Func<Task> callback)
        {
            throw new NotImplementedException();
        }

        public IConfigurableParameter<IFluentArgsBuilder<Action<bool>, Func<bool, Task>, bool>, bool> Flag(string name, params string[] moreNames)
        {
            throw new NotImplementedException();
        }

        public IParsable Invalid()
        {
            throw new NotImplementedException();
        }

        public IConfigurableParameter<IFluentArgsBuilder<Action<TParam>, Func<TParam, Task>, TParam>, TParam> Parameter<TParam>(string name, params string[] moreNames)
        {
            throw new NotImplementedException();
        }

        public IConfigurableParameter<IFluentArgsBuilder<Action<IReadOnlyList<TParam>>, Func<IReadOnlyList<TParam>, Task>, TParam>, TParam> ParameterList<TParam>(string name, params string[] moreNames)
        {
            throw new NotImplementedException();
        }

        public IFluentArgsBuilder<Action<bool>, Func<bool, Task>, bool> WithDescription(string description)
        {
            throw new NotImplementedException();
        }
    }
}
