namespace FluentArgs
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFluentArgsBuilder : IGivenAppliable<IFluentArgsBuilder>
    {
        IConfigurableParameter<IFluentArgsBuilder<Action<bool>, Func<bool, Task>, bool>, bool>
            Flag(string name, params string[] moreNames);

        IConfigurableParameter<IFluentArgsBuilder<Action<TParam>, Func<TParam, Task>, TParam>, TParam>
            Parameter<TParam>(string name, params string[] moreNames);

        IConfigurableParameter<IFluentArgsBuilder<Action<IReadOnlyList<TParam>>, Func<IReadOnlyList<TParam>, Task>, TParam>, TParam>
            ParameterList<TParam>(string name, params string[] moreNames);

        IParsable Call(Action callback);

        IParsable Call(Func<Task> callback);

        IParsable Invalid();
    }

    public interface IFluentArgsBuilder<TFunc, TFuncAsync, TParam> : IGivenAppliable<IFluentArgsBuilder<TFunc, TFuncAsync, TParam>>
    {
        IConfigurableParameter<IFluentArgsBuilder<Func<bool, TFunc>, Func<bool, TFuncAsync>, bool>, bool>
            Flag(string name, params string[] moreNames);

        IConfigurableParameter<IFluentArgsBuilder<Func<TNextParam, TFunc>, Func<TNextParam, TFuncAsync>, TNextParam>, TNextParam>
            Parameter<TNextParam>(string name, params string[] moreNames);

        IConfigurableParameter<IFluentArgsBuilder<Func<IReadOnlyList<TNextParam>, TFunc>, Func<IReadOnlyList<TNextParam>, TFuncAsync>, TNextParam>, TNextParam>
            ParameterList<TNextParam>(string name, params string[] moreNames);

        IParsable Call(TFunc callback);

        IParsable Call(TFuncAsync callback);

        IParsable Invalid();
    }

    public static class IFluentArgsBuilderExtensions
    {
        public static IConfigurableParameter<IFluentArgsBuilder<Action<string>, Func<string, Task>, string>, string>
            Parameter(this IFluentArgsBuilder builder, string name, params string[] moreNames)
        {
            return builder.Parameter<string>(name, moreNames);
        }

        public static IConfigurableParameter<IFluentArgsBuilder<Action<IReadOnlyList<string>>, Func<IReadOnlyList<string>, Task>, string>, string>
            ParameterList(this IFluentArgsBuilder builder, string name, params string[] moreNames)
        {
            return builder.ParameterList<string>(name, moreNames);
        }

        public static IConfigurableParameter<IFluentArgsBuilder<Func<string, TFunc>, Func<string, TFuncAsync>, string>, string>
            Parameter<TFunc, TFuncAsync, TParam>(this IFluentArgsBuilder<TFunc, TFuncAsync, TParam> builder, string name, params string[] moreNames)
        {
            return builder.Parameter<string>(name, moreNames);
        }

        public static IConfigurableParameter<IFluentArgsBuilder<Func<IReadOnlyList<string>, TFunc>, Func<IReadOnlyList<string>, TFuncAsync>, string>, string>
            ParameterList<TFunc, TFuncAsync, TParam>(this IFluentArgsBuilder<TFunc, TFuncAsync, TParam> builder, string name, params string[] moreNames)
        {
            return builder.ParameterList<string>(name, moreNames);
        }
    }
}
