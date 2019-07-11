namespace FluentArgs.Builder
{
    using System;

    internal class GivenThenBuilder<TArgsBuilder, TGiven> : IGivenThen<TArgsBuilder, TGiven>
    {
        private readonly Action<IParsable> thenExpressionBuilt;
        private readonly TArgsBuilder argsBuilder;
        private readonly Func<TGiven> result;

        public GivenThenBuilder(Action<IParsable> thenExpressionBuilt, TArgsBuilder argsBuilder, Func<TGiven> result)
        {
            this.thenExpressionBuilt = thenExpressionBuilt;
            this.argsBuilder = argsBuilder;
            this.result = result;
        }

        public TGiven Then(Func<TArgsBuilder, IBuildable> argumentBuilder)
        {
            thenExpressionBuilt(argumentBuilder(argsBuilder).Build());
            return result();
        }
    }
}
