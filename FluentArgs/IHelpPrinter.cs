namespace FluentArgs
{
    using System.Threading.Tasks;

    public interface IHelpPrinter
    {
        Task WriteTitle(string title);

        Task WriteDescription(string description);
    }
}
