using System.Collections.Immutable;

namespace FluentArgs.ArgumentExtraction
{
    public class ArgumentExtractor
    {
        private IImmutableList<IImmutableList<string>> argumentGroups;

        public ArgumentExtractor(string[] arguments)
        {
            argumentGroups = ImmutableList<IImmutableList<string>>.Empty.Add(arguments.ToImmutableList());
        }

        public bool Extract(string firstArgument, int followingArgumentsToInclude = 0)
        {
            foreach (var list in argumentGroups)
        }
    }
}
