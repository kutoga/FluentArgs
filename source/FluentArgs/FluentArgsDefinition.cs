namespace FluentArgs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Exceptions;
    using FluentArgs.Execution;
    using FluentArgs.Validation;

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
            return ParseFromState(State.InitialState(args, GetPostValidations(InitialStep.ParserSettings), InitialStep.ParserSettings?.AssignmentOperators));
        }

        public async Task<bool> ParseFromState(State state)
        {
            try
            {
                await ExecuteValidations().ConfigureAwait(false);
                await InitialStep.Execute(state).ConfigureAwait(false);
                return true;
            }
            catch (ArgumentMissingException ex)
            {
                await InitialStep.ParserSettings!.ParsingErrorPrinter.PrintArgumentMissingError(
                    ex.ArgumentName?.Names,
                    ex.Type,
                    ex.Description,
                    InitialStep.ParserSettings.HelpFlag?.Names).ConfigureAwait(false);
                return false;
            }
            catch (ArgumentParsingException ex)
            {
                await InitialStep.ParserSettings!.ParsingErrorPrinter.PrintArgumentParsingError(
                    ex.ArgumentName?.Names,
                    ex.TargetType,
                    ex.Description,
                    InitialStep.ParserSettings.HelpFlag?.Names).ConfigureAwait(false);
                return false;
            }
            catch (NotAllArgumentsUsedException ex)
            {
                await InitialStep.ParserSettings!.ParsingErrorPrinter.PrintNotAllArgumentsAreUsedError(
                    ex.UnusedArguments,
                    InitialStep.ParserSettings.HelpFlag?.Names).ConfigureAwait(false);
                return false;
            }
            catch (InvalidCommandValueException ex)
            {
                await InitialStep.ParserSettings!.ParsingErrorPrinter.PrintInvalidCommandValueError(
                    ex.CommandName.Names,
                    ex.Value,
                    InitialStep.ParserSettings.HelpFlag?.Names).ConfigureAwait(false);
                return false;
            }
        }

        private static IEnumerable<Action<State>> GetPostValidations(ParserSettings? settings)
        {
            if (settings == null)
            {
                yield break;
            }

            if (settings.DisallowUnusedArguments)
            {
                yield return state =>
                {
                    var remainingArguments = state.GetRemainingArguments(out _).ToArray();
                    if (remainingArguments.Any())
                    {
                        throw new NotAllArgumentsUsedException(remainingArguments);
                    }
                };
            }
        }

        private static IEnumerable<IStepVisitor> GetValidations(ParserSettings settings)
        {
            if (settings.ThrowOnDuplicateNames)
            {
                yield return new DuplicateNameDetection();
            }

            if (settings.ThrowOnNonMinusStartingNames)
            {
                yield return new NonMinusStartingNameDetection();
            }
        }

        private async Task ExecuteValidations()
        {
            if (InitialStep.ParserSettings == null)
            {
                return;
            }

            var validations = GetValidations(InitialStep.ParserSettings);
            foreach (var validation in validations)
            {
                await validation.Visit(InitialStep).ConfigureAwait(false);
            }
        }
    }
}