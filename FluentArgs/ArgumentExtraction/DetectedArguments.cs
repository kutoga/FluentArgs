namespace FluentArgs.ArgumentExtraction
{
    using System.Collections.Immutable;

    internal class DetectedArguments
    {
        public DetectedArguments(IImmutableList<string> arguments, IImmutableList<string> leftSideArguments, IImmutableList<string> rightSideArguments)
        {
            Arguments = arguments;
            LeftSideArguments = leftSideArguments;
            RightSideArguments = rightSideArguments;
        }

        public IImmutableList<string> Arguments { get; }

        public IImmutableList<string> LeftSideArguments { get; }

        public IImmutableList<string> RightSideArguments { get; }
    }
}
