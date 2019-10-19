namespace FluentArgs.Help
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Extensions;

    public class SimpleParsingErrorPrinter : IParsingErrorPrinter
    {
        private readonly ILineWriter errorLineWriter;

        public SimpleParsingErrorPrinter(TextWriter errorWriter)
        {
            errorLineWriter = new LineWriter(errorWriter);
        }

        public async Task PrintArgumentMissingError(IReadOnlyCollection<string>? aliases, Type targetType, string description, IReadOnlyCollection<string>? helpFlagAliases)
        {
            if (aliases != null)
            {
                await errorLineWriter
                    .WriteLine($"Required argument '{aliases.StringifyAliases()}' of type '{targetType.Name}' not found!")
                    .ConfigureAwait(false);
            }
            else
            {
                await errorLineWriter.WriteLine("Required positional argument not found!").ConfigureAwait(false);
            }

            await errorLineWriter.WriteLine($"Description: {description}").ConfigureAwait(false);
            await WriteHelpFlagInfo(helpFlagAliases).ConfigureAwait(false);
        }

        public async Task PrintArgumentParsingError(IReadOnlyCollection<string>? aliases, Type targetType, string description, IReadOnlyCollection<string>? helpFlagAliases)
        {
            if (aliases != null)
            {
                await errorLineWriter
                    .WriteLine($"Could not parse argument '{aliases.StringifyAliases()}' of type '{targetType.Name}'!")
                    .ConfigureAwait(false);
            }

            await errorLineWriter.WriteLine($"Error: {description}").ConfigureAwait(false);
            await WriteHelpFlagInfo(helpFlagAliases).ConfigureAwait(false);
        }

        public async Task PrintInvalidCommandValueError(IReadOnlyCollection<string> aliases, string value, IReadOnlyCollection<string>? helpFlagAliases)
        {
            await errorLineWriter
                .WriteLine($"The command '{aliases.StringifyAliases()}' has an invalid / unknown value: {value}")
                .ConfigureAwait(false);
            await WriteHelpFlagInfo(helpFlagAliases).ConfigureAwait(false);
        }

        public async Task PrintNotAllArgumentsAreUsedError(IReadOnlyCollection<string> remainingArguments, IReadOnlyCollection<string>? helpFlagAliases)
        {
            await errorLineWriter
                .WriteLine("There must not be any unused arguments, but the following parameters are not parsed / used:")
                .ConfigureAwait(false);
            await errorLineWriter
                .WriteLines(remainingArguments.Select(a => $" {a}"))
                .ConfigureAwait(false);
            await WriteHelpFlagInfo(helpFlagAliases).ConfigureAwait(false);
        }

        private Task WriteHelpFlagInfo(IReadOnlyCollection<string>? helpFlagAliases)
        {
            if (helpFlagAliases != null)
            {
                var currentApplicationName = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]);
                return errorLineWriter.WriteLines(
                    string.Empty,
                    "Show help for more information:",
                    $"  {currentApplicationName} {helpFlagAliases.AliasesOrdering().FirstOrDefault()}");
            }

            return Task.CompletedTask;
        }
    }
}