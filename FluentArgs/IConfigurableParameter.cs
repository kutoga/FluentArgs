namespace FluentArgs
{
    public interface IConfigurableParameter<TArgsBuilder, TParam> :
        IConfigurableParameterWithExamples<TArgsBuilder, TParam>
    {
        IConfigurableParameterWithExamples<TArgsBuilder, TParam> WithDescription(string description);
    }
}
