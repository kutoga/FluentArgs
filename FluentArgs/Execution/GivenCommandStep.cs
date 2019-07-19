﻿namespace FluentArgs.Execution
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
        //TODO: Put the branches & the name in a GivenCommand description or something like that
        private readonly Name name;
        private readonly IImmutableList<(GivenCommandBranch branch, IParsableFromState then)> branches;

        public GivenCommandStep(Step previousStep, Name name, IEnumerable<(GivenCommandBranch branch, IParsableFromState then)> branches)
            : base(previousStep)
        {
            this.name = name;
            this.branches = branches.ToImmutableList();
        }

        public override Task Execute(State state)
        {
            if (!state.TryExtractArguments(name.Names, out var arguments, out var newState, 1))
            {
                return Next.Execute(state);
            }
            else
            {
                var parameterValue = arguments[1];
                state = newState;

                foreach (var branch in branches)
                {
                    Func<State, string, GivenCommandBranch, IParsableFromState, (Task? result, bool matches)> handler;
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
                            throw new Exception("TODO: sinnvolle exception");
                    }

                    var (result, matches) = handler(state, parameterValue, branch.branch, branch.then);
                    //TODO: EInfach Task? zurückgeben; falls dieser null ist, wars nicht ok (=matches ist false)
                    if (matches)
                    {
                        return result;
                    }
                }
            }

            throw new Exception("TODO: something");
        }

        private (Task? result, bool matches) ExecuteHasValue(State state, string parameterValue, GivenCommandBranch branch, IParsableFromState then)
        {
            var value = Parse(parameterValue, branch.Parser, branch.ValueType);
            if (branch.PossibleValues.Any(p => object.Equals(value, p)))
            {
                return (then.ParseFromState(state), true);
            }

            return (default, false);
        }

        private (Task? result, bool matches) ExecuteMatches(State state, string parameterValue, GivenCommandBranch branch, IParsableFromState then)
        {
            var value = Parse(parameterValue, branch.Parser, branch.ValueType);
            var matches = branch.Predicate(value);
            Task? result = default;
            if (matches)
            {
                result = then.ParseFromState(state);
            }

            return (result, matches);
        }

        private (Task? result, bool matches) ExecuteIgnore(State state, string parameterValue, GivenCommandBranch branch, IParsableFromState then)
        {
            return (Next.Execute(state), true);
        }

        private (Task? result, bool matches) ExecuteInvalid(State state, string parameterValue, GivenCommandBranch branch, IParsableFromState then)
        {
            throw new Exception("TODO: invalid command value (show help?)");
        }

        private object Parse(string parameter, Func<string, object>? parser, Type type)
        {
            if (parser != null)
            {
                return parser(parameter);
            }

            if (DefaultStringParsers.TryGetParser(type, out var defaultParser))
            {
                return defaultParser!(parameter);
            }

            throw new Exception("TODO: IMPLEMENT MORE DEFAULTS");
        }
    }
}
