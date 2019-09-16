namespace FluentArgs.Execution
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using FluentArgs.ArgumentExtraction;

    internal class State
    {
        private readonly IImmutableList<object?> parameters;
        private readonly IArgumentExtractor argumentExtractor;

        private State(IImmutableList<object?> parameters, IArgumentExtractor argumentExtractor)
        {
            this.parameters = parameters;
            this.argumentExtractor = argumentExtractor;
        }

        private State(IImmutableList<object?> parameters, IImmutableList<string> arguments)
        {
            this.parameters = parameters;
            this.argumentExtractor = new ArgumentExtractor(arguments);
        }

        public static State InitialState(IEnumerable<string> arguments)
        {
            return new State(ImmutableList<object?>.Empty, arguments.ToImmutableList());
        }

        public State AddParameter(object? parameter)
        {
            return new State(parameters.Add(parameter), argumentExtractor);
        }

        public bool TryExtractArguments(IEnumerable<string> firstArgumentPossibilities, out IImmutableList<string> arguments, out State newState, int followingArgumentsToInclude = 0)
        {
            var result = argumentExtractor.TryExtract(firstArgumentPossibilities, out arguments, out var newArgumentExtractor, followingArgumentsToInclude);
            if (result)
            {
                newState = new State(parameters, newArgumentExtractor);
            }
            else
            {
                newState = default;
            }

            return result;
        }

        public IEnumerable<string> GetRemainingArguments(out State newState)
        {
            newState = new State(parameters, ArgumentExtractor.Empty);
            return argumentExtractor.GetRemainingArguments();
        }

        public IReadOnlyList<object?> GetParameters()
        {
            return parameters;
        }

        public bool PopArgument(out string? argument, out State newState)
        {
            if (parameters.Count == 0)
            {
                argument = default;
                newState = this;
                return false;
            }

            argument = parameters[0];
            return 
        }
    }
}
