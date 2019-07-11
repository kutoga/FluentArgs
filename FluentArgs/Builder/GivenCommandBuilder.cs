namespace FluentArgs.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentArgs.Description;
    using FluentArgs.Execution;

    internal class GivenCommandBuilder<TArgsBuilder> :
        IGivenCommand<TArgsBuilder>,
        IGivenThen<TArgsBuilder, IGivenCommand<TArgsBuilder>>
    {
        private readonly Name name;
        private readonly Func<TArgsBuilder> argsBuilderFactory;
        private readonly Step previousStep;
        //private GivenCommandBranch currentBranch;
        private IList<(GivenCommandBranch branch, IParsableFromState then)> branches;
        private readonly Func<Step, TArgsBuilder> stepWrapper;
        private GivenCommandBranch currentBranch;

        public GivenCommandBuilder(Name name, Func<TArgsBuilder> argsBuilderFactory, Step previousStep, Func<Step, TArgsBuilder> stepWrapper)
        {
            this.name = name;
            this.argsBuilderFactory = argsBuilderFactory;
            this.previousStep = previousStep;
            this.stepWrapper = stepWrapper;
            this.branches = new List<(GivenCommandBranch branch, IParsableFromState then)>();
        }

        public TArgsBuilder ElseIgnore()
        {
            //currentBranch = GivenCommandBranch.Ignore;
            branches.Add((GivenCommandBranch.Ignore, null));

            return stepWrapper(new GivenCommandStep(previousStep, name, branches));
        }

        public TArgsBuilder ElseIsInvalid()
        {
            //currentBranch = GivenCommandBranch.Invalid;
            branches.Add((GivenCommandBranch.Invalid, null));

            return stepWrapper(new GivenCommandStep(previousStep, name, branches));
        }

        public IGivenThen<TArgsBuilder, IGivenCommand<TArgsBuilder>> HasValue<TParam>(TParam value, Func<string, TParam> parser = null)
        {
            Func<string, object>? strParser = default; //TODO: Naming & Func<str, T> -> Func<str, obj>
            if (parser != null)
            {
                strParser = s => parser(s);
            }

            currentBranch = new GivenCommandBranch(GivenCommandBranchType.HasValue, value, typeof(TParam), strParser);
            return this;
        }

        public IGivenThen<TArgsBuilder, IGivenCommand<TArgsBuilder>> Matches<TParam>(Func<TParam, bool> predicate, Func<string, TParam> parser = null)
        {
            Func<string, object>? strParser = default; //TODO: Naming & Func<str, T> -> Func<str, obj>
            if (parser != null)
            {
                strParser = s => parser(s);
            }

            currentBranch = new GivenCommandBranch(GivenCommandBranchType.Matches, default, typeof(TParam), strParser, o => predicate((TParam)o));
            return this;
        }

        public IGivenCommand<TArgsBuilder> Then(Func<TArgsBuilder, IBuildable> argumentBuilder)
        {
            branches.Add((currentBranch, argumentBuilder(argsBuilderFactory()) as IParsableFromState));
            return this;

            //var result = argumentBuilder(argsBuilder);
            //var step = new GivenParameterStep(
            //        previousStep,
            //        GivenParameter.WithAnyValue(name),
            //        result as IParsableFromState ?? throw new Exception("TODO"));
            //return this;
        }
    }
}
