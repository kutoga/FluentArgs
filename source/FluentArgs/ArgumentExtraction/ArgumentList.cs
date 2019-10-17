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

        public IEnumerable<DetectedNamedArgument> DetectNamedArgument(string firstArgument, IReadOnlyCollection<string>? assignmentOperators)
        {
            var detectedAssignments = (assignmentOperators ?? Enumerable.Empty<string>()).SelectMany(o => Arguments
                .Select((a, i) => (argument: a, index: i))
                .Where(a => a.argument.StartsWith($"{firstArgument}{o}", StringComparison.InvariantCulture))
                .Select(a => new DetectedNamedArgument(
                    firstArgument,
                    a.argument.Substring($"{firstArgument}{o}".Length),
                    Arguments.Take(a.index).ToImmutableList(),
                    Arguments.Skip(a.index + 1).ToImmutableList())));

            var possibleIndices = Arguments
                .Select((a, i) => (argument: a, index: i))
                .Where(a => a.argument == firstArgument);

            var validIndices = possibleIndices
                .Where(a => (a.index + 1) < Arguments.Count)
                .Select(a => a.index);

            var detectedArguments = validIndices.Select(i => new DetectedNamedArgument(
                Arguments[i],
                Arguments[i + 1],
                Arguments.Take(i).ToImmutableList(),
                Arguments.Skip(i + 2).ToImmutableList()));

            return detectedArguments.Concat(detectedAssignments);
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
