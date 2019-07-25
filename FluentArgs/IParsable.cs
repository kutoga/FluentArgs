namespace FluentArgs
{
    using System.Threading.Tasks;

    public interface IParsable
    {
        void Parse(params string[] args);

        Task ParseAsync(params string[] args);
    }
}
