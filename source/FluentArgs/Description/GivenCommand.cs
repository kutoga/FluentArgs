namespace FluentArgs.Description
{
    using System.Collections.Generic;
    using System.Collections.Immutable;

    internal class GivenCommand
    {
        public GivenCommand(Name name, IEnumerable<GivenCommandBranch> branches)
        {
            Name = name;
            Branches = branches.ToImmutableList();
        }

        public Name Name { get; }

        public IReadOnlyList<GivenCommandBranch> Branches { get; }
    }
}
