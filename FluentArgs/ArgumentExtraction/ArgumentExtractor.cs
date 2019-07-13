namespace FluentArgs.ArgumentExtraction
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;

    internal class ArgumentExtractor
    {
        private IImmutableList<ArgumentList> argumentGroups;

        public ArgumentExtractor(string[] arguments)
        {
            argumentGroups = ImmutableList<ArgumentList>.Empty.Add(new ArgumentList(arguments.ToImmutableList()));
        }

        public bool TryExtract(string firstArgument, out IImmutableList<string> arguments, int followingArgumentsToInclude = 0)
        {
            return TryExtract(new[] { firstArgument }, out arguments, followingArgumentsToInclude);
        }

        public bool TryExtract(IEnumerable<string> firstArgumentPossibilities, out IImmutableList<string> arguments, int followingArgumentsToInclude = 0)
        {
            var detectedArgumentsPossibilities = firstArgumentPossibilities
                .SelectMany(firstArgument => DetectArguments(firstArgument, followingArgumentsToInclude))
                .ToImmutableList();

            if (detectedArgumentsPossibilities.Count == 0)
            {
                arguments = default;
                return false;
            }

            if (detectedArgumentsPossibilities.Count > 1)
            {
                arguments = default; //How to propagate this error to the outside world? 
                return false;
            }

            var foundArguments = detectedArgumentsPossibilities[0];
            foundArguments.splitArgumentList();
            arguments = foundArguments.arguments;
            return true;
        }

        private IEnumerable<(IImmutableList<string> arguments, Action splitArgumentList)> DetectArguments(
            string firstArgument,
            int followingArgumentsToInclude)
        {
            return argumentGroups
                .SelectMany(g => g.DetectArgument(firstArgument, followingArgumentsToInclude)
                    .Select(a =>
                    {
                        Action splitArgumentList = () => SplitArgumentList(g, a);
                        return (arguments: a.Arguments, splitArgumentList);
                    }));

            void SplitArgumentList(ArgumentList arguments, DetectedArguments detectedArguments)
            {
                var listToRemoveIndex = argumentGroups.IndexOf(arguments);
                var listsToInsert = new[] { detectedArguments.LeftSideArguments, detectedArguments.RightSideArguments }
                    .Where(a => a.Count > 0)
                    .Select(a => new ArgumentList(a))
                    .ToImmutableList(); //TODO: in the complete project: whenever possible, use immutable data structures

                argumentGroups = argumentGroups
                    .Take(listToRemoveIndex)
                    .Concat(listsToInsert)
                    .Concat(argumentGroups.Skip(listToRemoveIndex + 1))
                    .ToImmutableList();
            }
        }
    }
}
