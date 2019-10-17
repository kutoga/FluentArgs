namespace FluentArgs
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IInitialFluentArgsBuilder : IFluentArgsBuilder, IConfigurableParser<IInitialFluentArgsBuilder>
    {
    }
}
