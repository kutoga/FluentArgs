namespace FluentArgs.Description
{
    internal class ParserSettings
    {
        public ParserSettings(IHelpPrinter helpPrinter)
        {
            HelpPrinter = helpPrinter;
        }

        public string? ApplicationDescription { get; set; }

        public Name? HelpFlag { get; set; }

        public IHelpPrinter HelpPrinter { get; set; }

        public bool WarnOnDuplicateNames { get; set; }

        public bool WarnOnNonMinusStartingNames { get; set; }
    }
}
