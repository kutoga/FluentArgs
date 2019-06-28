namespace FluentArgs.Builder
{
    using System;
    using FluentArgs.Description;

    internal class ParameterListBuilder<TArgsBuilder, P> : IConfigurableParameter<TArgsBuilder, P>
    {
        private readonly Action<ParameterList> parameterListBuilt;
        private readonly ParameterList parameterList;
        private readonly TArgsBuilder argsBuilder;

        public ParameterListBuilder(Action<ParameterList> parameterBuilt, TArgsBuilder argsBuilder, Name parameterListName)
        {
            this.parameterListBuilt = parameterBuilt;
            this.argsBuilder = argsBuilder;
            parameterList = new ParameterList(parameterListName, typeof(P));
        }

        public TArgsBuilder IsOptional()
        {
            parameterList.IsRequired = false;
            return Finalize();
        }

        public TArgsBuilder IsOptionalWithDefault(P defaultValue)
        {
            parameterList.IsRequired = false;
            parameterList.HasDefaultValue = true;
            parameterList.DefaultValue = defaultValue;
            return Finalize();
        }

        public TArgsBuilder IsRequired()
        {
            parameterList.IsRequired = true;
            return Finalize();
        }

        public IConfigurableParameterWithExamples<TArgsBuilder, P> WithDescription(string description)
        {
            parameterList.Description = description;
            return this;
        }

        public IConfigurableParameterWithParser<TArgsBuilder, P> WithExamples(P example, params P[] moreExamples)
        {
            throw new NotImplementedException();
        }

        public IConfigurableParameterWithParser<TArgsBuilder, P> WithExamples(string example, params string[] moreExamples)
        {
            throw new NotImplementedException();
        }

        public IConfigurableParameterWithRequirement<TArgsBuilder, P> WithParser(Func<string, P> parser)
        {
            parameterList.Parser = s => parser(s);
            return this;
        }

        private TArgsBuilder Finalize()
        {
            parameterListBuilt(parameterList);
            return argsBuilder;
        }
    }
}
