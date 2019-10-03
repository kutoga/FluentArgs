namespace FluentArgs
{
    public interface IGivenAppliable<TArgsBuilder>
        where TArgsBuilder : class
    {
        IGiven<TArgsBuilder> Given { get; }
    }
}
