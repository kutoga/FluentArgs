using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentArgs.Extensions;

namespace FluentArgs.Help
{
    public class SimpleParsingErrorPrinter : IParsingErrorPrinter
    {
        private readonly ILineWriter errorLineWriter;
        private readonly IReadOnlyCollection<string>? helpFlagAliases;

        public SimpleParsingErrorPrinter(TextWriter errorWriter, IReadOnlyCollection<string>? helpFlagAlises)
        {
            errorLineWriter = new LineWriter(errorWriter);
            helpFlagAliases = helpFlagAlises;
        }

        public async Task PrintArgumentMissingError(IReadOnlyCollection<string> aliases, string description)
        {
            await errorLineWriter
                .WriteLine($"Required argument '{aliases.StringifyAliases()}' not found!")
                .ConfigureAwait(false);
            await WriteHelpFlagInfo().ConfigureAwait(false);
            await errorLineWriter.WriteLine($"Argument description: {description}").ConfigureAwait(false);
        }

        public async Task PrintArgumentParsingError(IReadOnlyCollection<string>? aliases, string description)
        {
            if (aliases != null)
            {
                await errorLineWriter
                    .WriteLine($"Could not parse argument '{aliases.StringifyAliases()}'!")
                    .ConfigureAwait(false);
                await WriteHelpFlagInfo().ConfigureAwait(false);
            }

            await errorLineWriter.WriteLine($"Parsing error: {description}").ConfigureAwait(false);
        }

        private Task WriteHelpFlagInfo()
        {
            if (helpFlagAliases != null)
                return errorLineWriter.WriteLines(
                    string.Empty,
                    $"Show help for more information: {Environment.GetCommandLineArgs()[0]} {helpFlagAliases.StringifyAliases()}");

            return Task.CompletedTask;
        }
    }
}