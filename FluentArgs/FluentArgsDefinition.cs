using System;
using System.Linq;
using System.Runtime.ExceptionServices;
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
            try
            {
                return ParseAsync(args).Result;
            }
            catch (AggregateException e)
            {
                // TODO: Is there a cleaner way to do this?
                if (e.InnerExceptions.Count != 1)
                {
                    throw new Exception("Multiple inner exceptions: This never should happen!", e);
                }

                ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                throw;
            }
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
                    ex.ArgumentName?.Names,
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