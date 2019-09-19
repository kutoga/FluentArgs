using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentArgs.Extensions;

namespace FluentArgs.Help
{
    public class SimpleParsingErrorPrinter : IParsingErrorPrinter
    {
        private readonly ILineWriter errorLineWriter;

        public SimpleParsingErrorPrinter(TextWriter errorWriter)
        {
            errorLineWriter = new LineWriter(errorWriter);
        }

        public async Task PrintArgumentMissingError(IReadOnlyCollection<string>? aliases, string description, IReadOnlyCollection<string>? helpFlagAliases)
        {
            if (aliases != null)
            {
                await errorLineWriter
                    .WriteLine($"Required argument '{aliases.StringifyAliases()}' not found!")
                    .ConfigureAwait(false);
            }
            else
            {
                await errorLineWriter.WriteLine("Required positional argument not found!").ConfigureAwait(false);
            }

            await errorLineWriter.WriteLine($"Description: {description}").ConfigureAwait(false);
            await WriteHelpFlagInfo(helpFlagAliases).ConfigureAwait(false);
        }

        public async Task PrintArgumentParsingError(IReadOnlyCollection<string>? aliases, string description, IReadOnlyCollection<string>? helpFlagAliases)
        {
            if (aliases != null)
            {
                await errorLineWriter
                    .WriteLine($"Could not parse argument '{aliases.StringifyAliases()}'!")
                    .ConfigureAwait(false);
                await WriteHelpFlagInfo(helpFlagAliases).ConfigureAwait(false);
            }

            await errorLineWriter.WriteLine($"Error: {description}").ConfigureAwait(false);
        }

        private Task WriteHelpFlagInfo(IReadOnlyCollection<string>? helpFlagAliases)
        {
            if (helpFlagAliases != null)
                return errorLineWriter.WriteLines(
                    string.Empty,
                    "Show help for more information:",
                    $"  {Environment.GetCommandLineArgs()[0]} {helpFlagAliases.AliasesOrdering().FirstOrDefault()}");

            return Task.CompletedTask;
        }
    }
}