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
            out string? value,
            out IArgumentExtractor newArgumentExtractor,
            IReadOnlyCollection<string>? assignmentOperators = null)
        {
            return TryExtractNamedArgument(
                new[] { firstArgument },
                out _,
                out value,
                out newArgumentExtractor,
                assignmentOperators);
        }

        public bool TryExtractFlag(IEnumerable<string> flagNamePossibilites, out string? flag, out IArgumentExtractor newArgumentExtractor)
        {
            var detectedArgumentsPossibilities = flagNamePossibilites
                .SelectMany(DetectFlagArgument)
                .ToImmutableList();

            if (detectedArgumentsPossibilities.Count == 0)
            {
                flag = default;
                newArgumentExtractor = this;
                return false;
            }

            var foundArgument = detectedArgumentsPossibilities[0];
            newArgumentExtractor = new ArgumentExtractor(foundArgument.splitArgumentList());
            flag = foundArgument.flagName;
            return true;
        }

        public bool TryExtractFlag(string flagName, out IArgumentExtractor newArgumentExtractor)
        {
            return TryExtractFlag(new[] { flagName }, out _, out newArgumentExtractor);
        }

        public bool TryPopArgument(out string? argument, out IArgumentExtractor newArgumentExtractor)
        {
            var firstArgumentGroupsWithElements = argumentGroups
                .Select((g, i) => (group: g, index: i))
                .SkipWhile(g => g.group.Arguments.Count == 0)
                .Select(g => (int?)g.index)
                .FirstOrDefault();

            if (firstArgumentGroupsWithElements == null)
            {
                argument = default;
                newArgumentExtractor = this;
                return false;
            }

            var argumentGroup = argumentGroups[firstArgumentGroupsWithElements.Value];
            argument = argumentGroup.Arguments[0];

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
            out string? argument,
            out string? value,
            out IArgumentExtractor newArgumentExtractor,
            IReadOnlyCollection<string>? assignmentOperators)
        {
            var detectedArgumentsPossibilities = firstArgumentPossibilities
                .SelectMany(firstArgument => DetectNamedArgument(firstArgument, assignmentOperators))
                .ToImmutableList();

            if (detectedArgumentsPossibilities.Count == 0)
            {
                argument = default;
                value = default;
                newArgumentExtractor = this;
                return false;
            }

            var foundArgument = detectedArgumentsPossibilities[0];
            newArgumentExtractor = new ArgumentExtractor(foundArgument.splitArgumentList());
            argument = foundArgument.argument;
            value = foundArgument.value;
            return true;
        }

        public IEnumerable<string> GetRemainingArguments()
        {
            return argumentGroups.SelectMany(g => g.Arguments);
        }

        private IEnumerable<(string argument, string value, Func<IEnumerable<ArgumentList>> splitArgumentList)> DetectNamedArgument(
            string argumentName,
            IReadOnlyCollection<string>? assignmentOperators)
        {
            return argumentGroups
                .SelectMany(g => g.DetectNamedArgument(argumentName, assignmentOperators)
                    .Select(a =>
                    {
                        Func<IEnumerable<ArgumentList>> splitArgumentList = () => SplitArgumentList(g, a);
                        return (argument: a.Argument, value: a.Value, splitArgumentList);
                    }));

            IEnumerable<ArgumentList> SplitArgumentList(ArgumentList arguments, DetectedNamedArgument detectedArguments)
            {
                var listToRemoveIndex = argumentGroups.IndexOf(arguments);
                var listsToInsert = new[] { detectedArguments.LeftSideArguments, detectedArguments.RightSideArguments }
                    .Where(a => a.Count > 0)
                    .Select(a => new ArgumentList(a))
                    .ToImmutableList();

                return argumentGroups
                    .Take(listToRemoveIndex)
                    .Concat(listsToInsert)
                    .Concat(argumentGroups.Skip(listToRemoveIndex + 1));
            }
        }

        private IEnumerable<(string flagName, Func<IEnumerable<ArgumentList>> splitArgumentList)> DetectFlagArgument(
            string flagName)
        {
            return argumentGroups
                .SelectMany(g => g.DetectFlagArgument(flagName)
                    .Select(a =>
                    {
                        Func<IEnumerable<ArgumentList>> splitArgumentList = () => SplitArgumentList(g, a);
                        return (argument: a.FlagName, splitArgumentList);
                    }));

            IEnumerable<ArgumentList> SplitArgumentList(ArgumentList arguments, DetectedFlagArgument detectedFlag)
            {
                var listToRemoveIndex = argumentGroups.IndexOf(arguments);
                var listsToInsert = new[] { detectedFlag.LeftSideArguments, detectedFlag.RightSideArguments }
                    .Where(a => a.Count > 0)
                    .Select(a => new ArgumentList(a))
                    .ToImmutableList();

                return argumentGroups
                    .Take(listToRemoveIndex)
                    .Concat(listsToInsert)
                    .Concat(argumentGroups.Skip(listToRemoveIndex + 1));
            }
        }
    }
}
