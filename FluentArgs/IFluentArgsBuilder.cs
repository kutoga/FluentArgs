namespace FluentArgs
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFluentArgsBuilder :
        IGivenAppliable<IFluentArgsBuilder>,
        ICallable<Action, Func<Task>>
    {
        IConfigurableFlagWithOptionalDescription
            Flag(string name, params string[] moreNames);

        IConfigurableParameter<IFluentArgsBuilder<Action<TParam>, Func<TParam, Task>>, TParam>
            Parameter<TParam>(string name, params string[] moreNames);

        IConfigurableParameterList<IFluentArgsBuilder<Action<IReadOnlyList<TParam>>, Func<IReadOnlyList<TParam>, Task>>, TParam>
            ParameterList<TParam>(string name, params string[] moreNames);

        IConfigurableRemainingArguments<Action<IReadOnlyList<TParam>>, Func<IReadOnlyList<TParam>, Task>, TParam> LoadRemainingArguments<TParam>();

        IParsable Invalid();
    }

    public interface IFluentArgsBuilder<TFunc, TFuncAsync> :
        IGivenAppliable<IFluentArgsBuilder<TFunc, TFuncAsync>>,
        ICallable<TFunc, TFuncAsync>
    {
        IConfigurableFlagWithOptionalDescription<TFunc, TFuncAsync>
            Flag(string name, params string[] moreNames);

        IConfigurableParameter<IFluentArgsBuilder<Func<TNextParam, TFunc>, Func<TNextParam, TFuncAsync>>, TNextParam>
            Parameter<TNextParam>(string name, params string[] moreNames);

        IConfigurableParameterList<IFluentArgsBuilder<Func<IReadOnlyList<TNextParam>, TFunc>, Func<IReadOnlyList<TNextParam>, TFuncAsync>>, TNextParam>
            ParameterList<TNextParam>(string name, params string[] moreNames);

        IConfigurableRemainingArguments<Func<IReadOnlyList<TParam>, TFunc>, Func<IReadOnlyList<TParam>, TFuncAsync>, TParam> LoadRemainingArguments<TParam>();

        IParsable Invalid();
    }

    public static class IFluentArgsBuilderExtensions
    {
        public static IConfigurableParameter<IFluentArgsBuilder<Action<string>, Func<string, Task>>, string>
            Parameter(this IFluentArgsBuilder builder, string name, params string[] moreNames)
        {
            return builder.Parameter<string>(name, moreNames);
        }

        public static IConfigurableParameterList<IFluentArgsBuilder<Action<IReadOnlyList<string>>, Func<IReadOnlyList<string>, Task>>, string>
            ParameterList(this IFluentArgsBuilder builder, string name, params string[] moreNames)
        {
            return builder.ParameterList<string>(name, moreNames);
        }

        public static IConfigurableRemainingArguments<Action<IReadOnlyList<string>>, Func<IReadOnlyList<string>, Task>, string>
            LoadRemainingArguments(this IFluentArgsBuilder builder)
        {
            return builder.LoadRemainingArguments<string>();
        }

        public static IConfigurableParameter<IFluentArgsBuilder<Func<string, TFunc>, Func<string, TFuncAsync>>, string>
            Parameter<TFunc, TFuncAsync>(this IFluentArgsBuilder<TFunc, TFuncAsync> builder, string name, params string[] moreNames)
        {
            return builder.Parameter<string>(name, moreNames);
        }

        public static IConfigurableParameterList<IFluentArgsBuilder<Func<IReadOnlyList<string>, TFunc>, Func<IReadOnlyList<string>, TFuncAsync>>, string>
            ParameterList<TFunc, TFuncAsync>(this IFluentArgsBuilder<TFunc, TFuncAsync> builder, string name, params string[] moreNames)
        {
            return builder.ParameterList<string>(name, moreNames);
        }

        public static IConfigurableRemainingArguments<Func<IReadOnlyList<string>, TFunc>, Func<IReadOnlyList<string>, TFuncAsync>, string>
            LoadRemainingArguments<TFunc, TFuncAsync>(this IFluentArgsBuilder<TFunc, TFuncAsync> builder)
        {
            return builder.LoadRemainingArguments<string>();
        }
    }
}
