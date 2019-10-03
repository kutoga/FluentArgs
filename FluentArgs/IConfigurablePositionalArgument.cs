namespace FluentArgs
{
    using System;

    public interface IConfigurablePositionalArgument<TArgsBuilder, TParam>
        : IWithConfigurableValidator<IConfigurablePositionalArgument<TArgsBuilder, TParam>, TParam>
        where TArgsBuilder : class
    {
        IConfigurablePositionalArgument<TArgsBuilder, TParam> WithDescription(string description);

        IConfigurablePositionalArgument<TArgsBuilder, TParam> WithExamples(TParam example, params TParam[] moreExamples);

        IConfigurablePositionalArgument<TArgsBuilder, TParam> WithExamples(string example, params string[] moreExamples);

        IConfigurablePositionalArgument<TArgsBuilder, TParam> WithParser(Func<string, TParam> parser);

        TArgsBuilder IsOptional();

        TArgsBuilder IsOptionalWithDefault(TParam defaultValue);

        TArgsBuilder IsRequired();
    }
}
