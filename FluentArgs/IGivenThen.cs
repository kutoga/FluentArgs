namespace FluentArgs
{
    using System;

    public interface IGivenThen<TArgsBuilder, TGiven>
    {
        TGiven Then(Func<TArgsBuilder, IParsable> argumentBuilder);
    }
}
