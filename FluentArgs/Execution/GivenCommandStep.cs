namespace FluentArgs.Execution
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Parser;

    internal class GivenCommandStep : Step
    {
        public GivenCommandStep(Step previousStep, Name name, IEnumerable<(GivenCommandBranch branch, IParsableFromState? then)> branches)
            : base(previousStep)
        {
            Name = name;
            Branches = branches.ToImmutableList();
        }

        // TODO: Put the branches & the name in a GivenCommand description or something like that
        public Name Name { get; }

        public IImmutableList<(GivenCommandBranch branch, IParsableFromState? then)> Branches { get; }

        public override Task Accept(IStepVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override Task Execute(State state)
        {
            if (!state.TryExtractNamedArgument(Name.Names, out _, out var value, out var newState))
            {
                return GetNextStep().Execute(state);
            }
            else
            {
                state = newState;

                foreach (var branch in Branches)
                {
                    Func<State, string, GivenCommandBranch, IParsableFromState?, Task?> handler;
                    switch (branch.branch.Type)
                    {
                        case GivenCommandBranchType.HasValue:
                            handler = ExecuteHasValue;
                            break;

                        case GivenCommandBranchType.Matches:
                            handler = ExecuteMatches;
                            break;

                        case GivenCommandBranchType.Ignore:
                            handler = ExecuteIgnore;
                            break;

                        case GivenCommandBranchType.Invalid:
                            handler = ExecuteInvalid;
                            break;

                        default:
                            throw new Exception("Invalid 'Given'-branch type.");
                    }

                    var result = handler(state, value!, branch.branch, branch.then);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            throw new Exception("Invalid GivenCommand step: Cannot continue!");
        }

        private Task? ExecuteHasValue(State state, string parameterValue, GivenCommandBranch branch, IParsableFromState? then)
        {
            if (branch.ValueType == null)
            {
                throw new ArgumentException("Value type cannot be null.");
            }

            var value = Parse(parameterValue, branch.Parser, branch.ValueType);
            if (branch.PossibleValues.Any(p => object.Equals(value, p)))
            {
                if (then == null)
                {
                    throw new Exception("No then-code defined!");
                }

                return then.ParseFromState(state);
            }

            return null;
        }

        private Task? ExecuteMatches(State state, string parameterValue, GivenCommandBranch branch, IParsableFromState? then)
        {
            if (branch.ValueType == null)
            {
                throw new ArgumentException("Value type cannot be null.");
            }

            var value = Parse(parameterValue, branch.Parser, branch.ValueType);
            var matches = branch.Predicate?.Invoke(value) ?? false;
            if (!matches)
            {
                return null;
            }

            if (then == null)
            {
                throw new Exception("No then-code defined!");
            }

            return then.ParseFromState(state);
        }

        private Task? ExecuteIgnore(State state, string parameterValue, GivenCommandBranch branch, IParsableFromState? then)
        {
            return GetNextStep().Execute(state);
        }

        private Task? ExecuteInvalid(State state, string parameterValue, GivenCommandBranch branch, IParsableFromState? then)
        {
            throw new ArgumentParsingException("Invalid command value.", Name);
        }

        private object Parse(string parameter, Func<string, object>? parser, Type type)
        {
            if (parser != null)
            {
                return parser(parameter);
            }

            if (DefaultStringParsers.TryGetParser(type, out var defaultParser))
            {
                return ArgumentParsingException.ParseWrapper(() => defaultParser!(parameter), Name);
            }

            throw ArgumentParsingException.NoParserFound(Name);
        }
    }
}
