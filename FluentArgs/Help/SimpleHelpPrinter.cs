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

        public Task WriteApplicationDescription(string description)
        {
            throw new NotImplementedException();
        }

        public Task WriteParameterInfos(string[] aliases, string description, bool optional, object defaultValue, object[] examples)
        {
            throw new NotImplementedException();
        }
    }
}
