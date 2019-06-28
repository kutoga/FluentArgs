namespace FluentArgs
{
    using System;

    public interface IConfigurableParameterWithParser<TArgsBuilder, TParam> :
        IConfigurableParameterWithRequirement<TArgsBuilder, TParam>
    {
        IConfigurableParameterWithRequirement<TArgsBuilder, TParam> WithParser(Func<string, TParam> parser);
    }
}
