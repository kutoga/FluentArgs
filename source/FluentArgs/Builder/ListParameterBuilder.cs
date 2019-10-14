namespace FluentArgs.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using FluentArgs.Description;
    using FluentArgs.Validation;

    internal class ListParameterBuilder<TArgsBuilder, TParam> : IConfigurableListParameter<TArgsBuilder, TParam>
    {
        private readonly Action<ListParameter> listParameterBuilt;
        private readonly ListParameter listParameter;
        private readonly TArgsBuilder argsBuilder;

        public ListParameterBuilder(Action<ListParameter> listParameterBuilt, TArgsBuilder argsBuilder, Name listParameterName)
        {
            this.listParameterBuilt = listParameterBuilt;
            this.argsBuilder = argsBuilder;
            listParameter = new ListParameter(listParameterName, typeof(TParam));
        }

        public TArgsBuilder IsOptional()
        {
            listParameter.IsRequired = false;
            return Finalize();
        }

        public TArgsBuilder IsOptionalWithDefault(IReadOnlyCollection<TParam> defaultValue)
        {
            listParameter.IsRequired = false;
            listParameter.HasDefaultValue = true;
            listParameter.DefaultValue = defaultValue;
            return Finalize();
        }

        public TArgsBuilder IsOptionalWithEmptyDefault()
        {
            listParameter.IsRequired = false;
            listParameter.HasDefaultValue = true;
            listParameter.DefaultValue = Array.CreateInstance(listParameter.Type, 0);
            return Finalize();
        }

        public TArgsBuilder IsRequired()
        {
            listParameter.IsRequired = true;
            return Finalize();
        }

        public IConfigurableListParameter<TArgsBuilder, TParam> WithDescription(string description)
        {
            listParameter.Description = description;
            return this;
        }

        public IConfigurableListParameter<TArgsBuilder, TParam> WithExamples(IReadOnlyCollection<TParam> example, params IReadOnlyCollection<TParam>[] moreExamples)
        {
            listParameter.Examples = Examples.Pack(example, moreExamples);
            return this;
        }

        public IConfigurableListParameter<TArgsBuilder, TParam> WithExamples(string example, params string[] moreExamples)
        {
            listParameter.Examples = Examples.Pack(example, moreExamples);
            return this;
        }

        public IConfigurableListParameter<TArgsBuilder, TParam> WithParser(Func<string, TParam> parser)
        {
            listParameter.Parser = s => parser(s) !;
            return this;
        }

        public IConfigurableListParameter<TArgsBuilder, TParam> WithSeparator(string separator, params string[] moreSeparators)
        {
            listParameter.Separators = new[] { separator }.Concat(moreSeparators).ToImmutableHashSet();
            return this;
        }

        public IConfigurableListParameter<TArgsBuilder, TParam> WithValidation(IValidation<TParam> validation)
        {
            listParameter.Validation = validation.ToObjectValidation();
            return this;
        }

        private TArgsBuilder Finalize()
        {
            listParameterBuilt(listParameter);
            return argsBuilder;
        }
    }
}
