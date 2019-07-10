namespace FluentArgs
{
    public interface ICallable<TFunc, TFuncAsync>
    {
        IParsable Call(TFunc callback);

        IParsable Call(TFuncAsync callback);
    }
}
