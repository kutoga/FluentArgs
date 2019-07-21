namespace FluentArgs.Help
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public class SimpleHelpPrinter : IHelpPrinter
    {
        public SimpleHelpPrinter(TextWriter outputWriter, TextWriter errorWriter)
        {
            OutputWriter = outputWriter;
            ErrorWriter = errorWriter;
        }

        public TextWriter OutputWriter { get; }

        public TextWriter ErrorWriter { get; }

        public Task WriteDescription(string description)
        {
            throw new NotImplementedException();
        }

        public Task WriteTitle(string title)
        {
            throw new NotImplementedException();
        }
    }
}
