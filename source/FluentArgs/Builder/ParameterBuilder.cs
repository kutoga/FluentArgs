namespace FluentArgs.Builder
{
    using System;
    using FluentArgs.Description;
    using FluentArgs.Validation;

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

        public IConfigurableParameter<TArgsBuilder, TParam> WithDescription(string description)
        {
            parameter.Description = description;
            return this;
        }

        public IConfigurableParameter<TArgsBuilder, TParam> WithExamples(TParam example, params TParam[] moreExamples)
        {
            parameter.Examples = Examples.Pack(example, moreExamples);
            return this;
        }

        public IConfigurableParameter<TArgsBuilder, TParam> WithExamples(string example, params string[] moreExamples)
        {
            parameter.Examples = Examples.Pack(example, moreExamples);
            return this;
        }

        public IConfigurableParameter<TArgsBuilder, TParam> WithParser(Func<string, TParam> parser)
        {
            parameter.Parser = s => parser(s) !;
            return this;
        }

        public IConfigurableParameter<TArgsBuilder, TParam> WithValidation(ValidationFunc<TParam> validation)
        {
            parameter.Validation = validation.ToObjectInput();
            return this;
        }

        private TArgsBuilder Finalize()
        {
            parameterBuilt(parameter);
            return argsBuilder;
        }
    }
}
