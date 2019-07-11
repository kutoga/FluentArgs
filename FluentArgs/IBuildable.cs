namespace FluentArgs
{
    using System.Threading.Tasks;

    public interface IBuildable
    {
        IParsable Build();
    }

    public static class ICallableExtensions
    {
        public static void Parse(this IBuildable buildable, string[] args)
        {
            buildable.Build().Parse(args);
        }

        public static Task ParseAsync(this IBuildable buildable, string[] args)
        {
            return buildable.Build().ParseAsync(args);
        }
    }
}
