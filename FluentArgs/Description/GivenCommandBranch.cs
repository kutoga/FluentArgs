namespace FluentArgs.Description
{
    using System;

    internal class GivenCommandBranch
    {
        public GivenCommandBranch(GivenCommandBranchType type, object[] possibleValues, Type valueType, Func<string, object>? parser, Func<object, bool>? predicate = null)
        {
            Type = type;
            PossibleValues = possibleValues;
            ValueType = valueType;
            Parser = parser;
            Predicate = predicate;
        }

        public static GivenCommandBranch Invalid { get; } = new GivenCommandBranch(GivenCommandBranchType.Invalid, default, default, default);

        public static GivenCommandBranch Ignore { get; } = new GivenCommandBranch(GivenCommandBranchType.Ignore, default, default, default);

        public GivenCommandBranchType Type { get; }

        public object[] PossibleValues { get; }

        public Type ValueType { get; }

        public Func<string, object>? Parser { get; }

        public Func<object, bool>? Predicate { get; }
    }
}
