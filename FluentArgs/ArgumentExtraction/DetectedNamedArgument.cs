namespace FluentArgs.ArgumentExtraction
{
    using System.Collections.Immutable;

    internal class DetectedNamedArgument
    {
        public DetectedNamedArgument(string argument, string value, IImmutableList<string> leftSideArguments, IImmutableList<string> rightSideArguments)
        {
            Argument = argument;
            Value = value;
            LeftSideArguments = leftSideArguments;
            RightSideArguments = rightSideArguments;
        }

        public string Argument { get; }

        public string Value { get; }

        public IImmutableList<string> LeftSideArguments { get; }

        public IImmutableList<string> RightSideArguments { get; }
    }
}
