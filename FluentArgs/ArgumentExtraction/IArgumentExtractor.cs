namespace FluentArgs.ArgumentExtraction
{
    using System.Collections.Generic;
    using System.Collections.Immutable;

    internal interface IArgumentExtractor
    {
        bool TryExtract(IEnumerable<string> firstArgumentPossibilities, out IImmutableList<string> arguments, out IArgumentExtractor newArgumentExtractor, int followingArgumentsToInclude = 0);

        bool TryExtract(string firstArgument, out IImmutableList<string> arguments, out IArgumentExtractor newArgumentExtractor, int followingArgumentsToInclude = 0);

        IEnumerable<string> GetRemainingArguments();
    }
}