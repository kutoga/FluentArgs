namespace FluentArgs
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFluentArgsBuilder :
        IGivenAppliable<IFluentArgsBuilder>,
        IPositionalArgumentFluentArgsBuilder
    {
        IConfigurableFlagWithOptionalDescription
            Flag(string name, params string[] moreNames);

        IConfigurableParameter<IFluentArgsBuilder<Action<TParam>, Func<TParam, Task>>, TParam>
            Parameter<TParam>(string name, params string[] moreNames);

        IConfigurableListParameter<IFluentArgsBuilder<Action<IReadOnlyList<TParam>>, Func<IReadOnlyList<TParam>, Task>>, TParam>
            ListParameter<TParam>(string name, params string[] moreNames);

        IBuildable Invalid();
    }

    public interface IFluentArgsBuilder<TFunc, TFuncAsync> :
        IGivenAppliable<IFluentArgsBuilder<TFunc, TFuncAsync>>,
        IPositionalArgumentFluentArgsBuilder<TFunc, TFuncAsync>
    {
        IConfigurableFlagWithOptionalDescription<TFunc, TFuncAsync>
            Flag(string name, params string[] moreNames);

        IConfigurableParameter<IFluentArgsBuilder<Func<TNextParam, TFunc>, Func<TNextParam, TFuncAsync>>, TNextParam>
            Parameter<TNextParam>(string name, params string[] moreNames);

        IConfigurableListParameter<IFluentArgsBuilder<Func<IReadOnlyList<TNextParam>, TFunc>, Func<IReadOnlyList<TNextParam>, TFuncAsync>>, TNextParam>
            ListParameter<TNextParam>(string name, params string[] moreNames);

        IBuildable Invalid();
    }

    public static class IFluentArgsBuilderExtensions
    {
        public static IConfigurableParameter<IFluentArgsBuilder<Action<string>, Func<string, Task>>, string>
            Parameter(this IFluentArgsBuilder builder, string name, params string[] moreNames)
        {
            return builder.Parameter<string>(name, moreNames);
        }

        public static IConfigurableListParameter<IFluentArgsBuilder<Action<IReadOnlyList<string>>, Func<IReadOnlyList<string>, Task>>, string>
            ListParameter(this IFluentArgsBuilder builder, string name, params string[] moreNames)
        {
            return builder.ListParameter<string>(name, moreNames);
        }

        public static IConfigurableParameter<IFluentArgsBuilder<Func<string, TFunc>, Func<string, TFuncAsync>>, string>
            Parameter<TFunc, TFuncAsync>(this IFluentArgsBuilder<TFunc, TFuncAsync> builder, string name, params string[] moreNames)
        {
            return builder.Parameter<string>(name, moreNames);
        }

        public static IConfigurableListParameter<IFluentArgsBuilder<Func<IReadOnlyList<string>, TFunc>, Func<IReadOnlyList<string>, TFuncAsync>>, string>
            ListParameter<TFunc, TFuncAsync>(this IFluentArgsBuilder<TFunc, TFuncAsync> builder, string name, params string[] moreNames)
        {
            return builder.ListParameter<string>(name, moreNames);
        }
    }
}
