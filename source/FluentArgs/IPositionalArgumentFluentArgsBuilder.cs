namespace FluentArgs
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPositionalArgumentFluentArgsBuilder :
        ICallable<Action, Func<Task>>
    {
        IConfigurablePositionalArgument<IPositionalArgumentFluentArgsBuilder<Action<TParam>, Func<TParam, Task>>, TParam>
            PositionalArgument<TParam>();

        IConfigurableRemainingArguments<Action<IReadOnlyList<TParam>>, Func<IReadOnlyList<TParam>, Task>, TParam> LoadRemainingArguments<TParam>();
    }

    public interface IPositionalArgumentFluentArgsBuilder<TFunc, TFuncAsync> :
        ICallable<TFunc, TFuncAsync>
    {
        IConfigurablePositionalArgument<IPositionalArgumentFluentArgsBuilder<Func<TNextParam, TFunc>, Func<TNextParam, TFuncAsync>>, TNextParam>
            PostionalArgument<TNextParam>();

        IConfigurableRemainingArguments<Func<IReadOnlyList<TParam>, TFunc>, Func<IReadOnlyList<TParam>, TFuncAsync>, TParam> LoadRemainingArguments<TParam>();
    }

    public static class IPositionalArgumentFluentArgsBuilderExtensions
    {
        public static IConfigurableRemainingArguments<Action<IReadOnlyList<string>>, Func<IReadOnlyList<string>, Task>, string>
            LoadRemainingArguments(this IPositionalArgumentFluentArgsBuilder builder)
        {
            return builder.LoadRemainingArguments<string>();
        }

        public static IConfigurablePositionalArgument<IPositionalArgumentFluentArgsBuilder<Action<string>, Func<string, Task>>, string>
            PositionalArgument(this IPositionalArgumentFluentArgsBuilder builder)
        {
            return builder.PositionalArgument<string>();
        }

        public static IConfigurableRemainingArguments<Func<IReadOnlyList<string>, TFunc>, Func<IReadOnlyList<string>, TFuncAsync>, string>
            LoadRemainingArguments<TFunc, TFuncAsync>(this IPositionalArgumentFluentArgsBuilder<TFunc, TFuncAsync> builder)
        {
            return builder.LoadRemainingArguments<string>();
        }

        public static IConfigurablePositionalArgument<IPositionalArgumentFluentArgsBuilder<Func<string, TFunc>, Func<string, TFuncAsync>>, string>
            PositionalArgument<TFunc, TFuncAsync>(this IPositionalArgumentFluentArgsBuilder<TFunc, TFuncAsync> builder)
        {
            return builder.PostionalArgument<string>();
        }
    }
}
