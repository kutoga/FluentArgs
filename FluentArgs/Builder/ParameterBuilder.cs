namespace FluentArgs.Builder
{
    using System;
    using FluentArgs.Description;

    internal class ParameterBuilder<TArgsBuilder, TParam> : IConfigurableParameter<TArgsBuilder, TParam>
    {
        private readonly Action<Parameter> parameterBuilt;
        private readonly Parameter parameter;
        private readonly TArgsBuilder argsBuilder;

        public ParameterBuilder(Action<Parameter> parameterBuilt, TArgsBuilder argsBuilder, Name parameterName)
        {
            this.parameterBuilt = parameterBuilt;
            this.argsBuilder = argsBuilder;
            parameter = new Parameter(parameterName, typeof(TParam));
        }

        public TArgsBuilder IsOptional()
        {
            parameter.IsRequired = false;
            return Finalize();
        }

        public TArgsBuilder IsOptionalWithDefault(TParam defaultValue)
        {
            parameter.IsRequired = false;
            parameter.HasDefaultValue = true;
            parameter.DefaultValue = defaultValue;
            return Finalize();
        }

        public TArgsBuilder IsRequired()
        {
            parameter.IsRequired = true;
            return Finalize();
        }

        public IConfigurableParameterWithExamples<TArgsBuilder, TParam> WithDescription(string description)
        {
            parameter.Description = description;
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
            parameter.Parser = s => parser(s);
            return this;
        }

        private TArgsBuilder Finalize()
        {
            parameterBuilt(parameter);
            return argsBuilder;
        }
    }
}
