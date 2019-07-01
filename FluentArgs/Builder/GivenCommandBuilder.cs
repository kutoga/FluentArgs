namespace FluentArgs.Builder
{
    using System;
    using System.Collections.Generic;
    using FluentArgs.Description;
    using FluentArgs.Execution;

    internal class GivenCommandBuilder<TArgsBuilder> :
        IGivenCommand<TArgsBuilder>,
        IGivenThen<TArgsBuilder, IGivenCommand<TArgsBuilder>>
    {
        private readonly Name name;
        private readonly TArgsBuilder argsBuilder;
        private readonly Step previousStep;
        //private GivenCommandBranch currentBranch;
        private IList<GivenCommandBranch> branches;

        public GivenCommandBuilder(Name name, TArgsBuilder argsBuilder, Step previousStep)
        {
            this.name = name;
            this.argsBuilder = argsBuilder;
            this.previousStep = previousStep;
            branches = new List<GivenCommandBranch>();
        }

        public TArgsBuilder ElseIgnore()
        {
            //currentBranch = GivenCommandBranch.Ignore;
            branches.Add(GivenCommandBranch.Ignore);
            var givenCommand = GetGivenCommand();
            return argsBuilder;
        }

        public TArgsBuilder ElseIsInvalid()
        {
            //currentBranch = GivenCommandBranch.Invalid;
            branches.Add(GivenCommandBranch.Invalid);
            var givenCommand = GetGivenCommand();
            return argsBuilder;
        }

        public IGivenThen<TArgsBuilder, IGivenCommand<TArgsBuilder>> HasValue<TParam>(TParam value, Func<string, TParam> parser = null)
        {
            Func<string, object>? strParser = default; //TODO: Naming & Func<str, T> -> Func<str, obj>
            if (parser != null)
            {
                strParser = s => parser(s);
            }

            branches.Add(new GivenCommandBranch(GivenCommandBranchType.HasValue, value, typeof(TParam), strParser));
            return this;
        }

        public IGivenThen<TArgsBuilder, IGivenCommand<TArgsBuilder>> Matches<TParam>(Func<TParam, bool> predicate, Func<string, TParam> parser = null)
        {
            Func<string, object>? strParser = default; //TODO: Naming & Func<str, T> -> Func<str, obj>
            if (parser != null)
            {
                strParser = s => parser(s);
            }

            branches.Add(new GivenCommandBranch(GivenCommandBranchType.Matches, default, typeof(TParam), strParser, o => predicate((TParam)o)));
            return this;
        }

        public IGivenCommand<TArgsBuilder> Then(Func<TArgsBuilder, IParsable> argumentBuilder)
        {
            var step = new GivenParameterStep(
                    previousStep,
                    GivenParameter.WithAnyValue(name),
                    parsable as IParsableFromState ?? throw new Exception("TODO")))

            throw new NotImplementedException();
        }

        private GivenCommand GetGivenCommand()
        {
            return new GivenCommand(name, branches);
        }
    }
}
