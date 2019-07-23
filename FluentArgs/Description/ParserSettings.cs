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

        //TODO: Write Visitor
        public bool WarnOnDuplicateNames { get; set; }

        //TODO: Write Visitor
        public bool WarnOnNonMinusStartingNames { get; set; }
    }
}
