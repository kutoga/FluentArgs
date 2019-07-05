namespace FluentArgs
{
    using System;
    using System.Threading.Tasks;

    public interface IConfigurableFlagWithOptionalDescription :
        IFluentArgsBuilder,
        IConfigurableFlag<IFluentArgsBuilder<Action<bool>, Func<bool, Task>, bool>>
    {
    }

    public interface IConfigurableFlagWithOptionalDescription<TFunc, TFuncAsync, TParam> :
        IFluentArgsBuilder<TFunc, TFuncAsync, TParam>,
        IConfigurableFlag<IFluentArgsBuilder<Func<bool, TFunc>, Func<bool, TFuncAsync>, bool>>
    {
    }
}
