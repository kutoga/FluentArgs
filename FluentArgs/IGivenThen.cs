namespace FluentArgs
{
    using System;
    using System.Threading.Tasks;

    public interface IGivenThen<TArgsBuilder, TGiven>
    {
        TGiven Then(Func<TArgsBuilder, IParsable> argumentBuilder);
    }

    public static class IGivenThenExtensions
    {
        public static TGiven Then<TGiven>(
            this IGivenThen<IFluentArgsBuilder, TGiven> givenThen,
            Action callback)
        {
            return givenThen.Then(b => b.Call(callback));
        }

        public static TGiven Then<TGiven>(
            this IGivenThen<IFluentArgsBuilder, TGiven> givenThen,
            Func<Task> callback)
        {
            return givenThen.Then(b => b.Call(callback));
        }

        public static TGiven Then<TGiven, TParam>(
            this IGivenThen<IFluentArgsBuilder<Action<TParam>, Func<TParam, Task>, TParam>, TGiven> givenThen,
            Action<TParam> callback)
        {
            return givenThen.Then(b => b.Call(callback));
        }

        public static TGiven Then<TGiven, TParam>(
            this IGivenThen<IFluentArgsBuilder<Action<TParam>, Func<TParam, Task>, TParam>, TGiven> givenThen,
            Func<TParam, Task> callback)
        {
            return givenThen.Then(b => b.Call(callback));
        }

        public static TGiven Then<TGiven, TNextParam, TFunc, TFuncAsync>(
            this IGivenThen<IFluentArgsBuilder<Func<TNextParam, TFunc>, Func<TNextParam, TFuncAsync>, TNextParam>,TGiven> givenThen,
            Func<TNextParam, TFunc> callback)
        {
            return givenThen.Then(b => b.Call(callback));
        }

        public static TGiven Then<TGiven, TNextParam, TFunc, TFuncAsync>(
            this IGivenThen<IFluentArgsBuilder<Func<TNextParam, TFunc>, Func<TNextParam, TFuncAsync>, TNextParam>, TGiven> givenThen,
            Func<TNextParam, TFuncAsync> callback)
        {
            return givenThen.Then(b => b.Call(callback));
        }
    }
}
