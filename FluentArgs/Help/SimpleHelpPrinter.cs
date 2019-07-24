namespace FluentArgs.Help
{
    using FluentArgs.Extensions;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SimpleHelpPrinter : IHelpPrinter
    {
        private const int MaxLineLength = 80;

        public SimpleHelpPrinter(TextWriter outputWriter, TextWriter errorWriter)
        {
            OutputWriter = outputWriter;
            ErrorWriter = errorWriter;
        }

        public TextWriter OutputWriter { get; }

        public TextWriter ErrorWriter { get; }

        public Task WriteApplicationDescription(string description)
        {
            return OutputWriter.WriteLines(SplitLine(description, MaxLineLength));
        }

        public Task WriteParameterInfos(IReadOnlyCollection<string> aliases, string description, bool optional, bool hasDefaultValue, object defaultValue, IReadOnlyCollection<string> examples)
        {
            throw new NotImplementedException();
        }

        public Task WriteParameterListInfos(IReadOnlyCollection<string> aliases, string description, bool optional, IReadOnlyCollection<string> separators, bool hasDefaultValue, object defaultValue, IReadOnlyCollection<string> examples)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<string> SplitLine(string line, int maxLineLength)
        {
            while (line.Length > maxLineLength)
            {
                var spaceIndices = line
                    .Take(maxLineLength)
                    .Select((c, i) => (character: c, index: i))
                    .Where(c => c.character == ' ')
                    .Select(c => c.index)
                    .ToList();

                var charsForCurrentLine = maxLineLength;
                if (spaceIndices.Count > 0)
                {
                    charsForCurrentLine = spaceIndices.Last() + 1;
                }

                yield return string.Concat(line.Take(charsForCurrentLine));
                line = string.Concat(line.Skip(charsForCurrentLine));
            }

            if (line.Length > 0)
            {
                yield return line;
            }
        }
    }
}
