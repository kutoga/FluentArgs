namespace FluentArgs.ArgumentExtraction
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;

    internal class ArgumentList
    {
        private readonly IImmutableList<string> arguments;

        public ArgumentList(IImmutableList<string> arguments)
        {
            this.arguments = arguments;
        }

        public IEnumerable<DetectedArguments> DetectArgument(string firstArgument, int followingNumberOfArguments)
        {
            if (followingNumberOfArguments < 0)
            {
                throw new Exception($"{nameof(followingNumberOfArguments)} must be non-negative!");
            }

            var possibelIndices = arguments
                .Select((a, i) => (argument: a, index: i))
                .Where(a => a.argument == firstArgument && (a.index + followingNumberOfArguments) < arguments.Count)
                .Select(a => a.index)
                .ToImmutableList();

            return possibelIndices.Select(i => new DetectedArguments(
                arguments.Skip(i).Take(1 + followingNumberOfArguments).ToImmutableList(),
                arguments.Take(i).ToImmutableList(),
                arguments.Skip(i + 1 + followingNumberOfArguments).ToImmutableList()));
        }
    }
}
