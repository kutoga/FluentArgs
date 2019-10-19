namespace FluentArgs
{
    public interface IConfigurableParser<TArgsParser>
        where TArgsParser : class, IConfigurableParser<TArgsParser> // TODO: überall wo möglich so reinmachen
    {
        TArgsParser WithApplicationDescription(string description);

        TArgsParser WithAssignmentOperators(string assignmentOperator, params string[] moreAssignmentOperators);

        TArgsParser WithoutAssignmentOperators();

        TArgsParser RegisterHelpFlag(string name, params string[] moreNames);

        TArgsParser RegisterHelpPrinter(IHelpPrinter helpPrinter);

        TArgsParser RegisterParsingErrorPrinter(IParsingErrorPrinter parsingErrorPrinter);

        TArgsParser ThrowOnDuplicateNames(bool enable = true);

        TArgsParser ThrowOnNonMinusStartingNames(bool enable = true);

        TArgsParser DisallowUnusedArguments(bool enable = true);
    }

    public static class IConfigurableParserExtensions
    {
        public static TArgsParser RegisterDefaultHelpFlags<TArgsParser>(
            this IConfigurableParser<TArgsParser> configurableParser)
            where TArgsParser : class, IConfigurableParser<TArgsParser>
        {
            return configurableParser.RegisterHelpFlag("-h", "--help");
        }

        public static TArgsParser DefaultConfigs<TArgsParser>(
            this IConfigurableParser<TArgsParser> configurableParser)
            where TArgsParser : class, IConfigurableParser<TArgsParser>
        {
            return configurableParser
                .RegisterDefaultHelpFlags()
                .ThrowOnDuplicateNames()
                .ThrowOnNonMinusStartingNames()
                .DisallowUnusedArguments();
        }

        public static TArgsParser DefaultConfigsWithAppDescription<TArgsParser>(
            this IConfigurableParser<TArgsParser> configurableParser, string description)
            where TArgsParser : class, IConfigurableParser<TArgsParser>
        {
            return configurableParser
                .DefaultConfigs()
                .WithApplicationDescription(description);
        }
    }
}
