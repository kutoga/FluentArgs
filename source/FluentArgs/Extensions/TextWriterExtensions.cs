namespace FluentArgs.Extensions
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    internal static class TextWriterExtensions
    {
        public static async Task WriteLines(this TextWriter textWriter, IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                await textWriter.WriteLineAsync(line).ConfigureAwait(false);
            }
        }
    }
}
