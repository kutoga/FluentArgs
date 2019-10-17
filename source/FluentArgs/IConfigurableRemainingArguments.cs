namespace FluentArgs
{
    using System;

    public interface IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam> : // TODO: überall den : auf die Zeile mit dem Klassennamen
        ICallable<TFunc, TFuncAsync>,
        IWithConfigurableValidation<IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam>, TParam>
    {
        IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam> WithDescription(string description);

        IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam> WithExamples(TParam example, params TParam[] moreExamples);

        IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam> WithExamples(string example, params string[] moreExamples);

        IConfigurableRemainingArguments<TFunc, TFuncAsync, TParam> WithParser(Func<string, TParam> parser);
    }
}
