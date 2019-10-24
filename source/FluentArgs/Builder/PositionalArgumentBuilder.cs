namespace FluentArgs.Builder
{
    using System;
    using FluentArgs.Description;
    using FluentArgs.Validation;

    internal class PositionalArgumentBuilder<TArgsBuilder, TParam> : IConfigurablePositionalArgument<TArgsBuilder, TParam>
        where TArgsBuilder : class
    {
        private readonly Action<PositionalArgument> positionalArgumentBuilt;
        private readonly PositionalArgument positionalArgument;
        private readonly TArgsBuilder argsBuilder;

        public PositionalArgumentBuilder(Action<PositionalArgument> positionalArgumentBuilt, TArgsBuilder argsBuilder)
        {
            this.positionalArgumentBuilt = positionalArgumentBuilt;
            this.argsBuilder = argsBuilder;
            positionalArgument = new PositionalArgument(typeof(TParam));
        }

        public TArgsBuilder IsOptional()
        {
            positionalArgument.IsRequired = false;
            return Finalize();
        }

        public TArgsBuilder IsOptionalWithDefault(TParam defaultValue)
        {
            positionalArgument.IsRequired = false;
            positionalArgument.HasDefaultValue = true;
            positionalArgument.DefaultValue = defaultValue;
            return Finalize();
        }

        public TArgsBuilder IsRequired()
        {
            positionalArgument.IsRequired = true;
            return Finalize();
        }

        public IConfigurablePositionalArgument<TArgsBuilder, TParam> WithDescription(string description)
        {
            positionalArgument.Description = description;
            return this;
        }

        public IConfigurablePositionalArgument<TArgsBuilder, TParam> WithExamples(TParam example, params TParam[] moreExamples)
        {
            positionalArgument.Examples = Examples.Pack(example, moreExamples);
            return this;
        }

        public IConfigurablePositionalArgument<TArgsBuilder, TParam> WithExamples(string example, params string[] moreExamples)
        {
            positionalArgument.Examples = Examples.Pack(example, moreExamples);
            return this;
        }

        public IConfigurablePositionalArgument<TArgsBuilder, TParam> WithParser(Func<string, TParam> parser)
        {
            positionalArgument.Parser = s => parser(s) !;
            return this;
        }

        public IConfigurablePositionalArgument<TArgsBuilder, TParam> WithValidation(ValidationFunc<TParam> validation)
        {
            positionalArgument.Validation = validation.ToObjectInput();
            return this;
        }

        private TArgsBuilder Finalize()
        {
            positionalArgumentBuilt(positionalArgument);
            return argsBuilder;
        }
    }
}
