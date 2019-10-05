namespace FluentArgs.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using FluentArgs.Description;
    using FluentArgs.Validation;

    internal class ParameterListBuilder<TArgsBuilder, TParam> : IConfigurableParameterList<TArgsBuilder, TParam>
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

        public TArgsBuilder IsOptionalWithDefault(IReadOnlyCollection<TParam> defaultValue)
        {
            parameterList.IsRequired = false;
            parameterList.HasDefaultValue = true;
            parameterList.DefaultValue = defaultValue;
            return Finalize();
        }

        public TArgsBuilder IsOptionalWithEmptyDefault()
        {
            parameterList.IsRequired = false;
            parameterList.HasDefaultValue = true;
            parameterList.DefaultValue = Array.CreateInstance(parameterList.Type, 0);
            return Finalize();
        }

        public TArgsBuilder IsRequired()
        {
            parameterList.IsRequired = true;
            return Finalize();
        }

        public IConfigurableParameterList<TArgsBuilder, TParam> WithDescription(string description)
        {
            parameterList.Description = description;
            return this;
        }

        public IConfigurableParameterList<TArgsBuilder, TParam> WithExamples(IReadOnlyCollection<TParam> example, params IReadOnlyCollection<TParam>[] moreExamples)
        {
            parameterList.Examples = Examples.Pack(example, moreExamples);
            return this;
        }

        public IConfigurableParameterList<TArgsBuilder, TParam> WithExamples(string example, params string[] moreExamples)
        {
            parameterList.Examples = Examples.Pack(example, moreExamples);
            return this;
        }

        public IConfigurableParameterList<TArgsBuilder, TParam> WithParser(Func<string, TParam> parser)
        {
            parameterList.Parser = s => parser(s) !;
            return this;
        }

        public IConfigurableParameterList<TArgsBuilder, TParam> WithSeparator(string separator, params string[] moreSeparators)
        {
            parameterList.Separators = new[] { separator }.Concat(moreSeparators).ToImmutableHashSet();
            return this;
        }

        public IConfigurableParameterList<TArgsBuilder, TParam> WithValidation(IValidation<TParam> validation)
        {
            parameterList.Validation = validation.ToObjectValidation();
            return this;
        }

        private TArgsBuilder Finalize()
        {
            parameterListBuilt(parameterList);
            return argsBuilder;
        }
    }
}
