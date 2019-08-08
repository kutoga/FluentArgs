﻿namespace FluentArgs.ArgumentExtraction
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

        public bool TryExtract(
            string firstArgument,
            out IImmutableList<string> arguments,
            out IArgumentExtractor newArgumentExtractor,
            int followingArgumentsToInclude = 0)
        {
            return TryExtract(new[] { firstArgument }, out arguments, out newArgumentExtractor, followingArgumentsToInclude);
        }

        public bool TryExtract(
            IEnumerable<string> firstArgumentPossibilities,
            out IImmutableList<string> arguments,
            out IArgumentExtractor newArgumentExtractor,
            int followingArgumentsToInclude = 0)
        {
            var detectedArgumentsPossibilities = firstArgumentPossibilities
                .SelectMany(firstArgument => DetectArguments(firstArgument, followingArgumentsToInclude))
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

        private IEnumerable<(IImmutableList<string> arguments, Func<IEnumerable<ArgumentList>> splitArgumentList)> DetectArguments(
            string firstArgument,
            int followingArgumentsToInclude)
        {
            return argumentGroups
                .SelectMany(g => g.DetectArgument(firstArgument, followingArgumentsToInclude)
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