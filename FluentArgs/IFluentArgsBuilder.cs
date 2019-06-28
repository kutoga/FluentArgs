namespace FluentArgs
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFluentArgsBuilder :
        IParsable, //TODO: FluentArgsBuilder.New().Parse() should not exist -> Remove IParsâble from all IFluentArgsBuilder
        IGivenAppliable<IFluentArgsBuilder>
    {
        IConfigurableParameter<IFluentArgsBuilder<bool>, bool>
            Flag(string name, params string[] moreNames);

        IConfigurableParameter<IFluentArgsBuilder<TParam1>, TParam1>
            Parameter<TParam1>(string name, params string[] moreNames);

        IConfigurableParameter<IFluentArgsBuilder<IReadOnlyList<TParam1>>, TParam1>
            ParameterList<TParam1>(string name, params string[] moreNames);

        IParsable Call(Action callback);

        IParsable Call(Func<Task> callback);
    }

    public interface IFluentArgsBuilder<TParam1> :
        IParsable,
        IConfigurableParameter<IFluentArgsBuilder<TParam1>, TParam1>,
        IGivenAppliable<IFluentArgsBuilder<TParam1>>
    {
        IConfigurableParameter<IFluentArgsBuilder<TParam1, bool>, bool>
            Flag(string name, params string[] moreNames);

        IConfigurableParameter<IFluentArgsBuilder<TParam1, TParam2>, TParam2>
            Parameter<TParam2>(string name, params string[] moreNames);

        IConfigurableParameter<IFluentArgsBuilder<TParam1, IReadOnlyList<TParam2>>, TParam2>
            ParameterList<TParam2>(string name, params string[] moreNames);

        IParsable Call(Action<TParam1> callback);

        IParsable Call(Func<TParam1, Task> callback);
    }

    public interface IFluentArgsBuilder<TParam1, TParam2> :
        IParsable,
        IConfigurableParameter<IFluentArgsBuilder<TParam1, TParam2>, TParam2>,
        IGivenAppliable<IFluentArgsBuilder<TParam1, TParam2>>
    {
        IConfigurableParameter<IFluentArgsBuilder<TParam1, TParam2, bool>, bool>
            Flag(string name, params string[] moreNames);

        IConfigurableParameter<IFluentArgsBuilder<TParam1, TParam2, TParam3>, TParam3>
            Parameter<TParam3>(string name, params string[] moreNames);

        IConfigurableParameter<IFluentArgsBuilder<TParam1, TParam2, IReadOnlyList<TParam3>>, TParam3>
            ParameterList<TParam3>(string name, params string[] moreNames);

        IParsable Call(Action<TParam1, TParam2> callback);

        IParsable Call(Func<TParam1, TParam2, Task> callback);
    }

    public interface IFluentArgsBuilder<TParam1, TParam2, TParam3> :
        IParsable,
        IConfigurableParameter<IFluentArgsBuilder<TParam1, TParam2, TParam3>, TParam3>,
        IGivenAppliable<IFluentArgsBuilder<TParam1, TParam2, TParam3>>
    {
        IParsable Call(Action<TParam1, TParam2, TParam3> callback);

        IParsable Call(Func<TParam1, TParam2, TParam3, Task> callback);
    }
}
