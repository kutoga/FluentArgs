namespace FluentArgs
{
    public interface IGiven<TArgsBuilder>
    {
        IGivenThen<TArgsBuilder, TArgsBuilder> Flag(string name, params string[] moreNames);

        IGivenParameter<TArgsBuilder> Parameter(string name, params string[] moreNames);

        IGivenCommandInitial<TArgsBuilder> Command(string name, params string[] moreNames);
    }
}
