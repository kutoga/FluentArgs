namespace FluentArgs.Description
{
    using System.Linq;

    internal class Name
    {
        public Name(string name, params string[] moreNames)
        {
            Names = moreNames.Concat(new[] { name })
                .OrderBy(n => n)
                .ToArray();
        }

        public string[] Names { get; }
    }
}
