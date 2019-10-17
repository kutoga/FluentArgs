namespace FluentArgs.ArgumentExtraction
{
    using System.Collections.Immutable;

    internal class DetectedFlagArgument
    {
        public DetectedFlagArgument(string flagName, IImmutableList<string> leftSideArguments, IImmutableList<string> rightSideArguments)
        {
            FlagName = flagName;
            LeftSideArguments = leftSideArguments;
            RightSideArguments = rightSideArguments;
        }

        public string FlagName { get; }

        public IImmutableList<string> LeftSideArguments { get; }

        public IImmutableList<string> RightSideArguments { get; }
    }
}
