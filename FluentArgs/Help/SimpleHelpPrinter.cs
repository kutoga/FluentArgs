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
        private readonly IList<(string parameterName, string description)> parameters;

        private ILineWriter OutputWriter => outputWriters.Peek();
        private ILineWriter ErrorWriter => errorWriters.Peek();

        public SimpleHelpPrinter(TextWriter outputWriter, TextWriter errorWriter)
        {
            outputWriters = new Stack<ILineWriter>();
            errorWriters = new Stack<ILineWriter>();
            parameters = new List<(string, string)>();
            outputWriters.Push(new LineWriter(outputWriter));
            errorWriters.Push(new LineWriter(errorWriter));
        }

        public async Task WriteApplicationDescription(string description)
        {
            await OutputWriter.WriteLines(SplitLine(description, MaxLineLength)).ConfigureAwait(false);
            await OutputWriter.WriteLine(string.Empty).ConfigureAwait(false);
        }

        public Task WriteParameterInfos(IReadOnlyCollection<string> aliases, string description, Type type, bool optional, bool hasDefaultValue, object defaultValue, IReadOnlyCollection<string> examples)
        {
            var aliasStr = string.Join("|", aliases.OrderBy(a => a.Length).ThenBy(a => a));
            var descriptionStr = "";
            if (optional)
            {
                if (hasDefaultValue)
                {
                    descriptionStr = $"Optional with default '{defaultValue}'. ";
                }
                else
                {
                    descriptionStr = "Optional. ";
                }
            }

            if (description != null)
            {
                descriptionStr += description + " ";
            }
            else
            {
                descriptionStr += $"Type: {type.Name} ";
            }

            if (examples.Count > 0)
            {
                descriptionStr += "Examples: " + string.Join(", ", examples);
            }
            else if (type.IsEnum)
            {
                descriptionStr += "Possible values: " + string.Join(", ", Enum.GetValues(type).Cast<object>().ToArray());
            }

            parameters.Add((aliasStr, descriptionStr));

            return Task.CompletedTask;
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

        public async Task Finalize()
        {
            if (parameters.Count == 0)
            {
                return;
            }

            var maxNameLength = parameters.Max(p => p.parameterName.Length);
            if (maxNameLength > 25)
            {
                foreach (var parameter in parameters)
                {
                    await OutputWriter.WriteLines(SplitLine(parameter.parameterName, MaxLineLength)).ConfigureAwait(false);
                    await OutputWriter.WriteLines(SplitLine(parameter.description, MaxLineLength - Tab.Length).Select(l => $"{Tab}{l}")).ConfigureAwait(false);
                }
            }
            else
            {
                var separator = " ";
                var descriptionLength = MaxLineLength - maxNameLength - separator.Length;
            }
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
