namespace FluentArgs
{
    public interface IGivenAppliable<TArgsBuilder>
    {
        IGiven<TArgsBuilder> Given { get; }
    }
}
