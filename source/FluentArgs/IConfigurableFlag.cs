namespace FluentArgs
{
    public interface IConfigurableFlag<TArgsBuilder>
    {
        TArgsBuilder WithDescription(string description);
    }
}
