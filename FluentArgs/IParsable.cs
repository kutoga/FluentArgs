namespace FluentArgs
{
    using System.Threading.Tasks;

    public interface IParsable
    {
        void Parse(string[] args);

        Task ParseAsync(string[] args);
    }
}
