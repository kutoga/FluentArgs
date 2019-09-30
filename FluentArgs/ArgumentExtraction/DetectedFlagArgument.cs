using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace FluentArgs.ArgumentExtraction
{
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
