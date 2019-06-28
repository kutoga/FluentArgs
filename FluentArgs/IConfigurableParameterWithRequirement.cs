namespace FluentArgs
{
    public interface IConfigurableParameterWithRequirement<TArgsBuilder, TParam>
    {
        TArgsBuilder IsOptional();

        TArgsBuilder IsOptionalWithDefault(TParam defaultValue);

        TArgsBuilder IsRequired();
    }
}
