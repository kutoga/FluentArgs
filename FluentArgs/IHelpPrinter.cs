namespace FluentArgs
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IHelpPrinter
    {
        Task WriteApplicationDescription(string description);

        Task WriteFlagInfos(
            IReadOnlyCollection<string> aliases,
            string? description,
            IReadOnlyCollection<(IReadOnlyCollection<string> aliases, string description)> givenHints);

        Task WriteParameterInfos(
            IReadOnlyCollection<string> aliases,
            string? description,
            Type type,
            bool optional,
            bool hasDefaultValue,
            object? defaultValue,
            IReadOnlyCollection<string> examples,
            IReadOnlyCollection<(IReadOnlyCollection<string> aliases, string description)> givenHints);

        Task WriteParameterListInfos(
            IReadOnlyCollection<string> aliases,
            string? description,
            Type type,
            bool optional,
            IReadOnlyCollection<string> separators,
            bool hasDefaultValue,
            object? defaultValue,
            IReadOnlyCollection<string> examples,
            IReadOnlyCollection<(IReadOnlyCollection<string> aliases, string description)> givenHints);

        Task WritePopArgumentInfos(
            string? description,
            Type type,
            bool optional,
            bool hasDefaultValue,
            object? defaultValue,
            IReadOnlyCollection<string> examples,
            IReadOnlyCollection<(IReadOnlyCollection<string> aliases, string description)> givenHints);

        Task WriteRemainingArgumentsAreUsed(
            string? description,
            Type type,
            IReadOnlyCollection<(IReadOnlyCollection<string> aliases, string description)> givenHints);

        Task Finalize();
    }
}
