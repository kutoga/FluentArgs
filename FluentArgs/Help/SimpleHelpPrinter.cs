namespace FluentArgs.Help
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class SimpleHelpPrinter : IHelpPrinter
    {
        private const string Tab = "    ";
        private const int MaxLineLength = 80;
        private readonly Stack<ILineWriter> outputWriters;
        private readonly Stack<ILineWriter> errorWriters;

        private ILineWriter OutputWriter => outputWriters.Peek();
        private ILineWriter ErrorWriter => errorWriters.Peek();

        public SimpleHelpPrinter(TextWriter outputWriter, TextWriter errorWriter)
        {
            this.outputWriters = new Stack<ILineWriter>();
            this.errorWriters = new Stack<ILineWriter>();
            this.outputWriters.Push(new LineWriter(outputWriter));
            this.errorWriters.Push(new LineWriter(errorWriter));
        }

        public async Task WriteApplicationDescription(string description)
        {
            await OutputWriter.WriteLines(SplitLine(description, MaxLineLength)).ConfigureAwait(false);
            await OutputWriter.WriteLine(string.Empty).ConfigureAwait(false);
        }

        public async Task WriteParameterInfos(IReadOnlyCollection<string> aliases, string description, Type type, bool optional, bool hasDefaultValue, object defaultValue, IReadOnlyCollection<string> examples)
        {
            var aliasStr = string.Join("|", aliases.OrderBy(a => a.Length).ThenBy(a => a));
            await OutputWriter.WriteLines(SplitLine($" {aliasStr}", MaxLineLength)).ConfigureAwait(false);
            using (AddTab())
            {
                await OutputWriter.WriteLine($"Optional: {(optional ? "yes" : "no")}").ConfigureAwait(false);
                if (hasDefaultValue)
                {
                    await OutputWriter.WriteLine($"Default value: {defaultValue}").ConfigureAwait(false);
                }

                if (examples.Count > 0)
                {
                    await OutputWriter.WriteLine("Example values:").ConfigureAwait(false);
                    using (AddTab())
                    {
                        await OutputWriter.WriteLines(examples.Select(e => $"{e}")).ConfigureAwait(false);
                    }
                }

                if (type.IsEnum)
                {
                    await OutputWriter.WriteLine("Possible values:").ConfigureAwait(false);
                    using (AddTab())
                    {
                        await OutputWriter.WriteLines(Enum.GetValues(type)
                            .Cast<object>()
                            .Select(t => t.ToString())).ConfigureAwait(false);
                    }
                }

                if (description != null)
                {
                    await OutputWriter.WriteLines(SplitLine(description, MaxLineLength)).ConfigureAwait(false);
                }

                //TODO: Write all enum values if the parameter type is enum
            }
        }

        public Task WriteParameterListInfos(IReadOnlyCollection<string> aliases, string description, Type type, bool optional, IReadOnlyCollection<string> separators, bool hasDefaultValue, object defaultValue, IReadOnlyCollection<string> examples)
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

        private IDisposable AddTab()
        {
            return new TabPrefixContext(this);
        }

        private class TabPrefixContext : IDisposable
        {
            private readonly SimpleHelpPrinter simpleHelpPrinter;

            public TabPrefixContext(SimpleHelpPrinter simpleHelpPrinter)
            {
                this.simpleHelpPrinter = simpleHelpPrinter;
                simpleHelpPrinter.outputWriters.Push(simpleHelpPrinter.OutputWriter.AddLinePrefix(Tab));
            }

            public void Dispose()
            {
                simpleHelpPrinter.outputWriters.Pop();
            }
        }
    }
}
