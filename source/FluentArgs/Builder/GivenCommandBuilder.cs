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
        where TArgsBuilder : class
    {
        private readonly Name name;
        private readonly Func<TArgsBuilder> argsBuilderFactory;
        private readonly Step previousStep;
        private readonly Func<Step, TArgsBuilder> stepWrapper;
        private IList<(GivenCommandBranch branch, IParsableFromState? then)> branches;
        private GivenCommandBranch? currentBranch;

        public GivenCommandBuilder(Name name, Func<TArgsBuilder> argsBuilderFactory, Step previousStep, Func<Step, TArgsBuilder> stepWrapper)
        {
            this.name = name;
            this.argsBuilderFactory = argsBuilderFactory;
            this.previousStep = previousStep;
            this.stepWrapper = stepWrapper;
            branches = new List<(GivenCommandBranch branch, IParsableFromState? then)>();
        }

        public TArgsBuilder ElseIgnore()
        {
            branches.Add((GivenCommandBranch.Ignore, null));

            return stepWrapper(new GivenCommandStep(previousStep, name, branches));
        }

        public TArgsBuilder ElseIsInvalid()
        {
            branches.Add((GivenCommandBranch.Invalid, null));

            return stepWrapper(new GivenCommandStep(previousStep, name, branches));
        }

        public IGivenThen<TArgsBuilder, IGivenCommand<TArgsBuilder>> HasValue<TParam>(TParam[] values, Func<string, TParam>? parser = null)
        {
            Func<string, object>? strParser = default;
            if (parser != null)
            {
                strParser = s => parser!(s) !;
            }

            currentBranch = new GivenCommandBranch(GivenCommandBranchType.HasValue, values.Cast<object>().ToArray(), typeof(TParam), strParser);
            return this;
        }

        public IGivenThen<TArgsBuilder, IGivenCommand<TArgsBuilder>> Matches<TParam>(Func<TParam, bool> predicate, Func<string, TParam>? parser = null)
        {
            Func<string, object>? strParser = default;
            if (parser != null)
            {
                strParser = s => parser!(s) !;
            }

            currentBranch = new GivenCommandBranch(GivenCommandBranchType.Matches, default, typeof(TParam), strParser, o => predicate((TParam)o));
            return this;
        }

        public IGivenCommand<TArgsBuilder> Then(Func<TArgsBuilder, IBuildable> argumentBuilder)
        {
            if (currentBranch == null)
            {
                throw new Exception("No branch defined! Cannot define a then-code!");
            }

            branches.Add((currentBranch, (IParsableFromState)argumentBuilder(argsBuilderFactory()).Build()));
            return this;
        }
    }
}
