namespace FluentArgs
{
    using System;
    using System.Threading.Tasks;

    public interface IConfigurableFlagWithOptionalDescription :
        IFluentArgsBuilder<Action<bool>, Func<bool, Task>>,
        IConfigurableFlag<IFluentArgsBuilder<Action<bool>, Func<bool, Task>>>
    {
    }

    public interface IConfigurableFlagWithOptionalDescription<TFunc, TFuncAsync> :
        IFluentArgsBuilder<Func<bool, TFunc>, Func<bool, TFuncAsync>>,
        IConfigurableFlag<IFluentArgsBuilder<Func<bool, TFunc>, Func<bool, TFuncAsync>>>
    {
    }
}
