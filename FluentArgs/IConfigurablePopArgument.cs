using System;
using System.Collections.Generic;
using System.Text;

namespace FluentArgs
{
    public interface IConfigurablePopArgument<TArgsBuilder, TParam>
    {
        IConfigurablePopArgument<TArgsBuilder, TParam> WithDescription(string description);

        IConfigurablePopArgument<TArgsBuilder, TParam> WithExamples(TParam example, params TParam[] moreExamples);

        IConfigurablePopArgument<TArgsBuilder, TParam> WithExamples(string example, params string[] moreExamples);

        IConfigurablePopArgument<TArgsBuilder, TParam> WithParser(Func<string, TParam> parser);

        TArgsBuilder IsOptional();

        TArgsBuilder IsOptionalWithDefault(TParam defaultValue);

        TArgsBuilder IsRequired();
    }
}
