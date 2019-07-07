namespace FluentArgs
{
    using System;
    using System.Threading.Tasks;

    public interface IConfigurableFlagWithOptionalDescription :
        IFluentArgsBuilder<Action<bool>, Func<bool, Task>, bool>,
        IConfigurableFlag<IFluentArgsBuilder<Action<bool>, Func<bool, Task>, bool>>
    {
    }

    public interface IConfigurableFlagWithOptionalDescription<TFunc, TFuncAsync> :
        IFluentArgsBuilder<Func<bool, TFunc>, Func<bool, TFuncAsync>, bool>,
        IConfigurableFlag<IFluentArgsBuilder<Func<bool, TFunc>, Func<bool, TFuncAsync>, bool>>
    {
    }
}
