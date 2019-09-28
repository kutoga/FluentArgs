using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using FluentArgs.Description;
using FluentArgs.Execution;
using FluentArgs.Extensions;
using FluentArgs.Help;
using FluentArgs.Validation;

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
            return ParseFromState(State.InitialState(args, InitialStep.ParserSettings?.AssignmentOperators));
        }

        public async Task<bool> ParseFromState(State state)
        {
            try
            {
                await ExecuteValidations();
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

        private async Task ExecuteValidations()
        {
            if (InitialStep.ParserSettings == null)
            {
                return;
            }

            var validators = GetValidators(InitialStep.ParserSettings);
            foreach (var validator in validators)
            {
                await validator.Visit(InitialStep).ConfigureAwait(false);
            }
        }

        private static IEnumerable<IStepVisitor> GetValidators(ParserSettings settings)
        {
            if (settings.WarnOnDuplicateNames)
            {
                yield return new DuplicateNameDetection();
            }
        }
    }
}