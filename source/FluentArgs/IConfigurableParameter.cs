namespace FluentArgs
{
    using System;

    public interface IConfigurableParameter<TArgsBuilder, TParam>
        : IWithConfigurableValidation<IConfigurableParameter<TArgsBuilder, TParam>, TParam>
    {
        IConfigurableParameter<TArgsBuilder, TParam> WithDescription(string description);

        IConfigurableParameter<TArgsBuilder, TParam> WithExamples(TParam example, params TParam[] moreExamples);

        IConfigurableParameter<TArgsBuilder, TParam> WithExamples(string example, params string[] moreExamples);

        IConfigurableParameter<TArgsBuilder, TParam> WithParser(Func<string, TParam> parser);

        TArgsBuilder IsOptional();

        TArgsBuilder IsOptionalWithDefault(TParam defaultValue);

        TArgsBuilder IsRequired();
    }
}
