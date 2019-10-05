namespace FluentArgs.Builder
{
    using System.Collections.Generic;
    using System.Linq;

    public static class Examples
    {
        public static IReadOnlyCollection<string> Pack(string example, params string[] moreExamples)
        {
            return new[] { example }.Concat(moreExamples).ToArray();
        }

        public static IReadOnlyCollection<string> Pack<T>(T example, params T[] moreExamples)
        {
            return new[] { example }.Concat(moreExamples).Select(e => $"{e}").ToArray();
        }
    }
}
