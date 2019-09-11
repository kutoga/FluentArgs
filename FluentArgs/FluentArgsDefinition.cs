using System;
using System.Linq;
using System.Threading.Tasks;
using FluentArgs.Description;
using FluentArgs.Execution;
using FluentArgs.Extensions;
using FluentArgs.Help;

namespace FluentArgs
{
    internal class FluentArgsDefinition : IParsableFromState
    {
        private readonly ILineWriter errorLineWriter;

        public FluentArgsDefinition(InitialStep initialStep, ILineWriter errorLineWriter)
        {
            InitialStep = initialStep;
            this.errorLineWriter = errorLineWriter;
        }

        public InitialStep InitialStep { get; }

        public bool Parse(params string[] args)
        {
            // TODO: unpack innerexception
            return ParseAsync(args).Result;
        }

        public Task<bool> ParseAsync(params string[] args)
        {
            return ParseFromState(State.InitialState(args));
        }

        public async Task<bool> ParseFromState(State state)
        {
            try
            {
                await InitialStep.Execute(state).ConfigureAwait(false);
                return true;
            }
            catch (ArgumentMissingException ex)
            {
                if (ex.ArgumentName != null)
                {
                    await errorLineWriter
                        .WriteLine($"Required argument '{ex.ArgumentName.Names.StringifyAliases()}' not found!")
                        .ConfigureAwait(false);
                    await WriteHelpFlagInfo().ConfigureAwait(false);
                }

                await errorLineWriter.WriteLine($"Argument description: {ex.Description}").ConfigureAwait(false);
                return false;
            }
            catch (ArgumentParsingException ex)
            {
                if (ex.ArgumentName != null)
                {
                    await errorLineWriter
                        .WriteLine($"Could not parse argument '{ex.ArgumentName.Names.StringifyAliases()}'!")
                        .ConfigureAwait(false);
                    await WriteHelpFlagInfo().ConfigureAwait(false);
                }

                await errorLineWriter.WriteLine($"Parsing error: {ex.Description}").ConfigureAwait(false);
                return false;
            }
        }

        private Task WriteHelpFlagInfo()
        {
            if (InitialStep.ParserSettings.HelpFlag != null)
            {
                return errorLineWriter.WriteLines(
                    string.Empty,
                    $"Show help for more information: {Environment.GetCommandLineArgs()[0]} {InitialStep.ParserSettings.HelpFlag.Names.StringifyAliases()}");
            }

            return Task.CompletedTask;
        }
    }
}