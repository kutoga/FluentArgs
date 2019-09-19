namespace FluentArgs.ArgumentExtraction
{
    using System.Collections.Generic;
    using System.Collections.Immutable;

    internal interface IArgumentExtractor
    {
        bool TryExtractNamedArgument(IEnumerable<string> firstArgumentPossibilities, out string argument, out string value, out IArgumentExtractor newArgumentExtractor);

        bool TryExtractNamedArgument(string firstArgument, out string value, out IArgumentExtractor newArgumentExtractor);

        bool TryExtractFlag(IEnumerable<string> flagNamePossibilites, out string flag, out IArgumentExtractor newArgumentExtractor);

        bool TryExtractFlag(string flagName, out IArgumentExtractor newArgumentExtractor);


        bool TryPopArgument(out string argument, out IArgumentExtractor newArgumentExtractor);

        IEnumerable<string> GetRemainingArguments();
    }
}