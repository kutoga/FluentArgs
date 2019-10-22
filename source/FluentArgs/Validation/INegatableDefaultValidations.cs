namespace FluentArgs.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface INegatableDefaultValidations : IDefaultValidatons
    {
        IDefaultValidatons Not { get; }
    }
}
