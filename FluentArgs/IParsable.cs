namespace FluentArgs
{
    using System.Threading.Tasks;

    public interface IParsable
    {
        bool Parse(params string[] args);

        Task<bool> ParseAsync(params string[] args);
    }
}
