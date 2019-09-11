namespace FluentArgs
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICallable<TFunc, TFuncAsync>
    {
        IBuildable Call(TFunc callback);

        IBuildable Call(TFuncAsync callback);

        IBuildable CallUntyped(Action<IReadOnlyCollection<object?>> callback);

        IBuildable CallUntyped(Func<IReadOnlyCollection<object?>, Task> callback);
    }
}
