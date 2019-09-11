using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentArgs
{
    public interface IParsingErrorPrinter
    {
        Task PrintArgumentMissingError(
            IReadOnlyCollection<string> aliases,
            string description,
            IReadOnlyCollection<string>? helpFlagAliases);

        Task PrintArgumentParsingError(
            IReadOnlyCollection<string>? aliases,
            string description,
            IReadOnlyCollection<string>? helpFlagAliases);
    }
}
