namespace FluentArgs
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFluentArgsBuilder :
        IParsable, //TODO: FluentArgsBuilder.New().Parse() should not exist -> Remove IParsâble from all IFluentArgsBuilder
        IGivenAppliable<IFluentArgsBuilder>
    {
        IConfigurableParameter<IFluentArgsBuilder<Action<bool>, Func<bool, Task>, bool>, bool>
            Flag(string name, params string[] moreNames);

        IConfigurableParameter<IFluentArgsBuilder<Action<TParam1>, Func<TParam1, Task>, TParam1>, TParam1>
            Parameter<TParam1>(string name, params string[] moreNames);

        IConfigurableParameter<IFluentArgsBuilder<Action<IReadOnlyList<TParam1>>, Func<IReadOnlyList<TParam1>, Task>, TParam1>, TParam1>
            ParameterList<TParam1>(string name, params string[] moreNames);

        IParsable Call(Action callback);

        IParsable Call(Func<Task> callback);
    }

    public interface IFluentArgsBuilder<TFunc, TFuncAsync, TParam> :
        IParsable,
        IGivenAppliable<IFluentArgsBuilder<TFunc, TFuncAsync, TParam>>
    {
        IConfigurableParameter<IFluentArgsBuilder<Func<bool, TFunc>, Func<bool, TFuncAsync>, bool>, bool>
            Flag(string name, params string[] moreNames);

        IConfigurableParameter<IFluentArgsBuilder<Func<TNextParam, TFunc>, Func<TNextParam, TFuncAsync>, TNextParam>, TNextParam>
            Parameter<TNextParam>(string name, params string[] moreNames);

        IConfigurableParameter<IFluentArgsBuilder<Func<IReadOnlyList<TNextParam>, TFunc>, Func<IReadOnlyList<TNextParam>, TFuncAsync>, TNextParam>, TNextParam>
            ParameterList<TNextParam>(string name, params string[] moreNames);

        IParsable Call(TFunc callback);

        IParsable Call(TFuncAsync callback);
    }
}
