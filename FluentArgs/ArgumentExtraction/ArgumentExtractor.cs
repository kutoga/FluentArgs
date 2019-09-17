namespace FluentArgs.ArgumentExtraction
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;

    internal class ArgumentExtractor : IArgumentExtractor
    {
        private readonly IImmutableList<ArgumentList> argumentGroups;

        public ArgumentExtractor(IEnumerable<string> arguments)
            : this(new[] { new ArgumentList(arguments.ToImmutableList()) })
        {
        }

        private ArgumentExtractor(IEnumerable<ArgumentList> argumentLists)
        {
            argumentGroups = argumentLists.ToImmutableList();
        }

        public static ArgumentExtractor Empty { get; } = new ArgumentExtractor(Enumerable.Empty<string>());

        public bool TryExtractNamedArgument(
            string firstArgument,
            out IImmutableList<string> arguments,
            out IArgumentExtractor newArgumentExtractor)
        {
            return TryExtractNamedArgument(new[] { firstArgument }, out arguments, out newArgumentExtractor);
        }

        public bool TryExtractFlag(IEnumerable<string> flagNamePossibilites, out string flag, out IArgumentExtractor newArgumentExtractor)
        {
            throw new NotImplementedException();
        }

        public bool TryExtractFlag(string flagName, out IArgumentExtractor newArgumentExtractor)
        {
            throw new NotImplementedException();
        }

        public bool TryPopArgument(out string argument, out IArgumentExtractor newArgumentExtractor)
        {
            var firstArgumentGroupsWithElements = argumentGroups
                .Select((g, i) => (group: g, index: i))
                .SkipWhile(g => g.group.Arguments.Count == 0)
                .Select(g => (int?) g.index)
                .FirstOrDefault();

            if (firstArgumentGroupsWithElements == null)
            {
                argument = default;
                newArgumentExtractor = default; // TODO: default OR newArgumentExtractor?
                return false;
            }

            var argumentGroup = argumentGroups[firstArgumentGroupsWithElements.Value];
            argument = argumentGroup.Arguments.First();

            if (argumentGroup.Arguments.Count == 1)
            {
                newArgumentExtractor = new ArgumentExtractor(argumentGroups.RemoveAt(firstArgumentGroupsWithElements.Value));
            }
            else
            {
                var newArgumentGroups = argumentGroups.SetItem(
                    firstArgumentGroupsWithElements.Value,
                    new ArgumentList(argumentGroup.Arguments.Skip(1).ToImmutableList()));
                newArgumentExtractor = new ArgumentExtractor(newArgumentGroups);
            }

            return true;
        }

        public bool TryExtractNamedArgument(
            IEnumerable<string> firstArgumentPossibilities,
            out IImmutableList<string> arguments,
            out IArgumentExtractor newArgumentExtractor)
        {
            var detectedArgumentsPossibilities = firstArgumentPossibilities
                .SelectMany(firstArgument => DetectNamedArgument(firstArgument, followingArgumentsToInclude))
                .ToImmutableList();

            if (detectedArgumentsPossibilities.Count == 0)
            {
                arguments = default;
                newArgumentExtractor = default;
                return false;
            }

            if (detectedArgumentsPossibilities.Count > 1)
            {
                arguments = default; //How to propagate this error to the outside world? 
                newArgumentExtractor = default;
                return false;
            }

            var foundArguments = detectedArgumentsPossibilities[0];
            newArgumentExtractor = new ArgumentExtractor(foundArguments.splitArgumentList());
            arguments = foundArguments.arguments;
            return true;
        }

        public IEnumerable<string> GetRemainingArguments()
        {
            return argumentGroups.SelectMany(g => g.Arguments);
        }

        private IEnumerable<(IImmutableList<string> arguments, Func<IEnumerable<ArgumentList>> splitArgumentList)> DetectNamedArgument(
            string argumentName)
        {
            return argumentGroups
                .SelectMany(g => g.DetectNamedArgument(argumentName)
                    .Select(a =>
                    {
                        Func<IEnumerable<ArgumentList>> splitArgumentList = () => SplitArgumentList(g, a);
                        return (arguments: a.Arguments, splitArgumentList);
                    }));

            IEnumerable<ArgumentList> SplitArgumentList(ArgumentList arguments, DetectedArguments detectedArguments)
            {
                var listToRemoveIndex = argumentGroups.IndexOf(arguments);
                var listsToInsert = new[] { detectedArguments.LeftSideArguments, detectedArguments.RightSideArguments }
                    .Where(a => a.Count > 0)
                    .Select(a => new ArgumentList(a))
                    .ToImmutableList(); //TODO: in the complete project: whenever possible, use immutable data structures

                return argumentGroups
                    .Take(listToRemoveIndex)
                    .Concat(listsToInsert)
                    .Concat(argumentGroups.Skip(listToRemoveIndex + 1));
            }
        }
    }
}
