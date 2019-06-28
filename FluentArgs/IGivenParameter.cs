namespace FluentArgs
{
    using System;

    public interface IGivenParameter<TArgsBuilder>
    {
        IGivenThen<TArgsBuilder, TArgsBuilder> WithValue<TParam>(TParam value, Func<string, TParam>? parser = null);

        IGivenThen<TArgsBuilder, TArgsBuilder> WithAnyValue();
    }
}
