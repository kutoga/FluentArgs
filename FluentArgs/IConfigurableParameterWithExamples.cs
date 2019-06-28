namespace FluentArgs
{
    public interface IConfigurableParameterWithExamples<TArgsBuilder, TParam> :
        IConfigurableParameterWithParser<TArgsBuilder, TParam>
    {
        IConfigurableParameterWithParser<TArgsBuilder, TParam> WithExamples(TParam example, params TParam[] moreExamples);

        IConfigurableParameterWithParser<TArgsBuilder, TParam> WithExamples(string example, params string[] moreExamples);
    }
}
