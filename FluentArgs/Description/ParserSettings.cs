using System.Collections.Generic;
using System.Collections.Immutable;

namespace FluentArgs.Description
{
    internal class ParserSettings
    {
        public ParserSettings(IHelpPrinter helpPrinter, IParsingErrorPrinter parsingErrorPrinter)
        {
            HelpPrinter = helpPrinter;
            ParsingErrorPrinter = parsingErrorPrinter;
            AssignmentOperators = ImmutableArray<string>.Empty;
        }

        public string? ApplicationDescription { get; set; }

        public IReadOnlyCollection<string> AssignmentOperators { get; set; }

        public Name? HelpFlag { get; set; }

        public IHelpPrinter HelpPrinter { get; set; }

        public IParsingErrorPrinter ParsingErrorPrinter { get; set; }

        //TODO: Write Visitor
        public bool WarnOnDuplicateNames { get; set; }

        //TODO: Write Visitor
        public bool WarnOnNonMinusStartingNames { get; set; }
    }
}
