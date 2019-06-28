using System;

namespace FluentArgs
{
    public interface IGivenCommand<TArgsBuilder>
    {
        IGivenCommandValue<TArgsBuilder> HasValue<TParam>(TParam value, Func<string, TParam>? parser = null);

        IGivenCommandValue<TArgsBuilder> Matches<TParam>(Func<TParam, bool> predicate, Func<string, TParam>? parser = null);

        IParsable ElseIsInvalid();

        TArgsBuilder ElseIgnore();
    }
}
