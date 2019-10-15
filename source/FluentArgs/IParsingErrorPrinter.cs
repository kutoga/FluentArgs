namespace FluentArgs
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IParsingErrorPrinter
    {
        Task PrintArgumentMissingError(
            IReadOnlyCollection<string>? aliases,
            Type targetType,
            string description,
            IReadOnlyCollection<string>? helpFlagAliases);

        Task PrintArgumentParsingError(
            IReadOnlyCollection<string>? aliases,
            Type targetType,
            string description,
            IReadOnlyCollection<string>? helpFlagAliases);

        Task PrintInvalidCommandValueError(
            IReadOnlyCollection<string> aliases,
            string value,
            IReadOnlyCollection<string>? helpFlagAliases);
    }
}
