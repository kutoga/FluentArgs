using System.Threading.Tasks;

namespace FluentArgs
{
    public interface ICallable<TFunc, TFuncAsync>
    {
        IBuildable Call(TFunc callback);

        IBuildable Call(TFuncAsync callback);
    }
}
