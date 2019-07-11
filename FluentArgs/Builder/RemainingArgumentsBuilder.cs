namespace FluentArgs.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal class RemainingArgumentsBuilder<TFunc, TFuncAsync, TParam> : IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam>
    {
        public IParsable Call(TFunc callback)
        {
            throw new NotImplementedException();
        }

        public IParsable Call(TFuncAsync callback)
        {
            throw new NotImplementedException();
        }

        public IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam> WithDescription(string description)
        {
            throw new NotImplementedException();
        }

        public IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam> WithExamples(TParam example, params TParam[] moreExamples)
        {
            throw new NotImplementedException();
        }

        public IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam> WithExamples(string example, params string[] moreExamples)
        {
            throw new NotImplementedException();
        }

        public IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam> WithParser(Func<string, TParam> parser)
        {
            throw new NotImplementedException();
        }
    }
}
