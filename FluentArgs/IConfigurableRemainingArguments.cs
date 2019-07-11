namespace FluentArgs
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam> : //TODO: überall den : auf die Zeile mit dem Klassennamen
        ICallable<TFunc, TFuncAsync>
    {
        IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam> WithDescription(string description);

        IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam> WithExamples(TParam example, params TParam[] moreExamples);

        IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam> WithExamples(string example, params string[] moreExamples);

        IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam> WithParser(Func<string, TParam> parser);
    }
}
