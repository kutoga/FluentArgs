namespace FluentArgs.Help
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Extensions;

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

        public static Task WriteLines(this ILineWriter writer, params string[] lines)
        {
            return writer.WriteLines(lines.AsEnumerable());
        }

        public static ILineWriter AddLinePrefix(this ILineWriter writer, string linePrefix)
        {
            return new PrefixedLineWriter(writer, linePrefix);
        }

        private class PrefixedLineWriter : ILineWriter
        {
            private readonly ILineWriter wrapped;
            private readonly string prefix;

            public PrefixedLineWriter(ILineWriter wrapped, string prefix)
            {
                this.wrapped = wrapped;
                this.prefix = prefix;
            }

            public Task WriteLine(string line)
            {
                return wrapped.WriteLine(prefix + line);
            }
        }
    }
}
