namespace FluentArgs.Builder
{
    using System;
    using FluentArgs.Description;

    internal class PopArgumentBuilder<TArgsBuilder, TParam> : IConfigurablePopArgument<TArgsBuilder, TParam>
    {
        private readonly Action<PopArgument> popArgumentBuilt;
        private readonly PopArgument popArgument;
        private readonly TArgsBuilder argsBuilder;

        public PopArgumentBuilder(Action<PopArgument> popArgumentBuilt, TArgsBuilder argsBuilder)
        {
            this.popArgumentBuilt = popArgumentBuilt;
            this.argsBuilder = argsBuilder;
            popArgument = new PopArgument(typeof(TParam));
        }

        public TArgsBuilder IsOptional()
        {
            popArgument.IsRequired = false;
            return Finalize();
        }

        public TArgsBuilder IsOptionalWithDefault(TParam defaultValue)
        {
            popArgument.IsRequired = false;
            popArgument.HasDefaultValue = true;
            popArgument.DefaultValue = defaultValue;
            return Finalize();
        }

        public TArgsBuilder IsRequired()
        {
            popArgument.IsRequired = true;
            return Finalize();
        }

        public IConfigurablePopArgument<TArgsBuilder, TParam> WithDescription(string description)
        {
            popArgument.Description = description;
            return this;
        }

        public IConfigurablePopArgument<TArgsBuilder, TParam> WithExamples(TParam example, params TParam[] moreExamples)
        {
            throw new NotImplementedException();
        }

        public IConfigurablePopArgument<TArgsBuilder, TParam> WithExamples(string example, params string[] moreExamples)
        {
            throw new NotImplementedException();
        }

        public IConfigurablePopArgument<TArgsBuilder, TParam> WithParser(Func<string, TParam> parser)
        {
            popArgument.Parser = s => parser(s);
            return this;
        }

        private TArgsBuilder Finalize()
        {
            popArgumentBuilt(popArgument);
            return argsBuilder;
        }
    }
}
