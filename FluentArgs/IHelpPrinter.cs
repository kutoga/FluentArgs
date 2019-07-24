namespace FluentArgs
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IHelpPrinter
    {
        Task WriteApplicationDescription(string description);

        Task WriteParameterInfos(
            IReadOnlyCollection<string> aliases,
            string? description,
            bool optional,
            bool hasDefaultValue,
            object? defaultValue,
            IReadOnlyCollection<string> examples);

        Task WriteParameterListInfos(
            IReadOnlyCollection<string> aliases,
            string? description,
            bool optional,
            IReadOnlyCollection<string> separators,
            bool hasDefaultValue,
            object? defaultValue,
            IReadOnlyCollection<string> examples);
    }
}
