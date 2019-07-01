namespace FluentArgs.Builder
{
    using System;
    using FluentArgs.Description;

    internal class ParameterListBuilder<TArgsBuilder, TParam> : IConfigurableParameter<TArgsBuilder, TParam>
    {
        private readonly Action<ParameterList> parameterListBuilt;
        private readonly ParameterList parameterList;
        private readonly TArgsBuilder argsBuilder;

        public ParameterListBuilder(Action<ParameterList> parameterBuilt, TArgsBuilder argsBuilder, Name parameterListName)
        {
            this.parameterListBuilt = parameterBuilt;
            this.argsBuilder = argsBuilder;
            parameterList = new ParameterList(parameterListName, typeof(TParam));
        }

        public TArgsBuilder IsOptional()
        {
            parameterList.IsRequired = false;
            return Finalize();
        }

        public TArgsBuilder IsOptionalWithDefault(TParam defaultValue)
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

        public IConfigurableParameterWithExamples<TArgsBuilder, TParam> WithDescription(string description)
        {
            parameterList.Description = description;
            return this;
        }

        public IConfigurableParameterWithParser<TArgsBuilder, TParam> WithExamples(TParam example, params TParam[] moreExamples)
        {
            throw new NotImplementedException();
        }

        public IConfigurableParameterWithParser<TArgsBuilder, TParam> WithExamples(string example, params string[] moreExamples)
        {
            throw new NotImplementedException();
        }

        public IConfigurableParameterWithRequirement<TArgsBuilder, TParam> WithParser(Func<string, TParam> parser)
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
