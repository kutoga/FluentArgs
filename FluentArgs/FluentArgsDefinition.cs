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
        public FluentArgsDefinition(InitialStep initialStep)
        {
            InitialStep = initialStep;
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
                await InitialStep.ParserSettings.ParsingErrorPrinter.PrintArgumentMissingError(
                    ex.ArgumentName.Names,
                    ex.Description,
                    InitialStep.ParserSettings.HelpFlag?.Names).ConfigureAwait(false);
                return false;
            }
            catch (ArgumentParsingException ex)
            {
                await InitialStep.ParserSettings.ParsingErrorPrinter.PrintArgumentParsingError(
                    ex.ArgumentName?.Names,
                    ex.Description,
                    InitialStep.ParserSettings.HelpFlag?.Names).ConfigureAwait(false);
                return false;
            }
        }
    }
}