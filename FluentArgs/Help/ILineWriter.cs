namespace FluentArgs.Help
{
    using FluentArgs.Extensions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    internal interface ILineWriter
    {
        Task WriteLine(string line);
    }

    internal static class ILineWriterExtensions
    {
        public static Task WriteLines(this ILineWriter writer, IEnumerable<string> lines)
        {
            return lines.Select(writer.WriteLine).Serialize();
        }

        public static ILineWriter AddLinePrefix(this ILineWriter writer, string linePrefix)
        {
            return new PrefixedLineWriter(writer, linePrefix);
        }

        private class PrefixedLineWriter : ILineWriter
        {
            private readonly ILineWriter wrapped;
            private readonly string prefix;

            public PrefixedLineWriter(ILineWriter wrapper, string prefix)
            {
                this.wrapped = wrapper;
                this.prefix = prefix;
            }

            public Task WriteLine(string line)
            {
                return wrapped.WriteLine(prefix + line);
            }
        }
    }
}
