namespace FluentArgs.Builder
{
    using System;
    using System.Collections.Generic;
    using FluentArgs.Description;

    internal class GivenCommandBuilder<TArgsBuilder> : IGivenCommand<TArgsBuilder>
    {
        private readonly Name name;
        private readonly TArgsBuilder argsBuilder;
        private IList<GivenCommandBranch> branches;

        public GivenCommandBuilder(Name name, TArgsBuilder argsBuilder)
        {
            this.name = name;
            this.argsBuilder = argsBuilder;
            branches = new List<GivenCommandBranch>();
        }

        public TArgsBuilder ElseIgnore()
        {
            throw new NotImplementedException();
        }

        public TArgsBuilder ElseIsInvalid()
        {
            throw new NotImplementedException();
        }

        public IGivenThen<TArgsBuilder, IGivenCommand<TArgsBuilder>> HasValue<TParam>(TParam value, Func<string, TParam> parser = null)
        {
            throw new NotImplementedException();
        }

        public IGivenThen<TArgsBuilder, IGivenCommand<TArgsBuilder>> Matches<TParam>(Func<TParam, bool> predicate, Func<string, TParam> parser = null)
        {
            throw new NotImplementedException();
        }
    }
}
