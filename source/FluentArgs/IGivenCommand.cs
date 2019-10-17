namespace FluentArgs
{
    using System;

    public interface IGivenCommand<TArgsBuilder> : IGivenCommandInitial<TArgsBuilder>
    {
        TArgsBuilder ElseIsInvalid();

        TArgsBuilder ElseIgnore();
    }
}
