namespace FluentArgs
{
    using System.Threading.Tasks;

    public interface IBuildable
    {
        IParsable Build();
    }

    public static class ICallableExtensions
    {
        public static bool Parse(this IBuildable buildable, params string[] args)
        {
            return buildable.Build().Parse(args);
        }

        public static Task<bool> ParseAsync(this IBuildable buildable, params string[] args)
        {
            return buildable.Build().ParseAsync(args);
        }
    }
}
