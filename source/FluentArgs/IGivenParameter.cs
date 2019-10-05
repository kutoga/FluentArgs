namespace FluentArgs
{
    using System;

    public interface IGivenParameter<TArgsBuilder>
        where TArgsBuilder : class
    {
        IGivenThen<TArgsBuilder, TArgsBuilder> HasValue<TParam>(TParam value, Func<string, TParam>? parser = null);

        IGivenThen<TArgsBuilder, TArgsBuilder> Exists();
    }
}
