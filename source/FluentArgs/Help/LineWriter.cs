namespace FluentArgs.Help
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    internal class LineWriter : ILineWriter
    {
        private readonly TextWriter writer;

        public LineWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        public Task WriteLine(string line)
        {
            return writer.WriteLineAsync(line);
        }
    }
}
