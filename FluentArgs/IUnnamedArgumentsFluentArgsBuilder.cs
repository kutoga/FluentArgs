using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentArgs
{
    public interface IUnnamedArgumentsFluentArgsBuilder :
        ICallable<Action, Func<Task>>
    {
        IConfigurablePopArgument<IUnnamedArgumentsFluentArgsBuilder<Action<TParam>, Func<TParam, Task>>, TParam>
            PopArgument<TParam>();

        IConfigurableRemainingArguments<Action<IReadOnlyList<TParam>>, Func<IReadOnlyList<TParam>, Task>, TParam> LoadRemainingArguments<TParam>();
    }

    public interface IUnnamedArgumentsFluentArgsBuilder<TFunc, TFuncAsync> :
        ICallable<TFunc, TFuncAsync>
    {
        IConfigurablePopArgument<IUnnamedArgumentsFluentArgsBuilder<Func<TNextParam, TFunc>, Func<TNextParam, TFuncAsync>>, TNextParam>
            PopArgument<TNextParam>();

        IConfigurableRemainingArguments<Func<IReadOnlyList<TParam>, TFunc>, Func<IReadOnlyList<TParam>, TFuncAsync>, TParam> LoadRemainingArguments<TParam>();
    }

    public static class IUnnamedArgumentsFluentArgsBuilderExtensions
    {
        public static IConfigurableRemainingArguments<Action<IReadOnlyList<string>>, Func<IReadOnlyList<string>, Task>, string>
            LoadRemainingArguments(this IUnnamedArgumentsFluentArgsBuilder builder)
        {
            return builder.LoadRemainingArguments<string>();
        }

        public static IConfigurablePopArgument<IUnnamedArgumentsFluentArgsBuilder<Action<string>, Func<string, Task>>, string>
            PopArgument(this IUnnamedArgumentsFluentArgsBuilder builder)
        {
            return builder.PopArgument<string>();
        }

        public static IConfigurableRemainingArguments<Func<IReadOnlyList<string>, TFunc>, Func<IReadOnlyList<string>, TFuncAsync>, string>
            LoadRemainingArguments<TFunc, TFuncAsync>(this IUnnamedArgumentsFluentArgsBuilder<TFunc, TFuncAsync> builder)
        {
            return builder.LoadRemainingArguments<string>();
        }

        public static IConfigurablePopArgument<IUnnamedArgumentsFluentArgsBuilder<Func<string, TFunc>, Func<string, TFuncAsync>>, string>
            PopArgument<TFunc, TFuncAsync>(this IUnnamedArgumentsFluentArgsBuilder<TFunc, TFuncAsync> builder)
        {
            return builder.PopArgument<string>();
        }
    }
}
