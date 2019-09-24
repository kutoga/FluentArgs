namespace FluentArgs.ArgumentExtraction
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;

    internal class ArgumentList
    {
        public ArgumentList(IImmutableList<string> arguments)
        {
            this.Arguments = arguments;
        }

        public IImmutableList<string> Arguments { get; }

        public IEnumerable<DetectedNamedArgument> DetectNamedArgument(string firstArgument)
        {
            var possibleIndices = Arguments
                .Select((a, i) => (argument: a, index: i))
                .Where(a => a.argument == firstArgument);
            //.ToImmutableList(); //TODO: Use ToList (instead of ToImmutableList) everywhere (where possible)
            //var invalidIndices = possibleIndices
            //    .Where(a => (a.index + followingNumberOfArguments) >= Arguments.Count)
            //    .ToList();

            //TODO: cleanup
            //if (invalidIndices.Count > 0)
            //{
            //    throw new Exception($"Found argument '{firstArgument}', but its parameters are not present!");
            //}

            var validIndices = possibleIndices
                .Where(a => (a.index + 1) < Arguments.Count)
                .Select(a => a.index);
                //.ToList();

            return validIndices.Select(i => new DetectedNamedArgument(
                Arguments[i],
                Arguments[i + 1],
                Arguments.Take(i).ToImmutableList(),
                Arguments.Skip(i + 2).ToImmutableList()));
        }

        public IEnumerable<DetectedFlagArgument> DetectFlagArgument(string flagName)
        {
            var validIndices = Arguments
                .Select((a, i) => (argument: a, index: i))
                .Where(a => a.argument == flagName)
                .Select(a => a.index);

            return validIndices.Select(i => new DetectedFlagArgument(
                Arguments[i],
                Arguments.Take(i).ToImmutableList(),
                Arguments.Skip(i + 1).ToImmutableList()));
        }
    }
}
