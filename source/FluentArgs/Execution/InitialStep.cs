namespace FluentArgs.Execution
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Help;

    internal class InitialStep : Step
    {
        public ParserSettings? ParserSettings { get; set; }

        public override Task Accept(IStepVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override Task Execute(State state)
        {
            if (ShowHelp(state))
            {
                var helpGenerator = new HelpGenerator(this);
                return helpGenerator.Write();
            }

            if (Next == null)
            {
                throw new Exception("TODO: Good message");
            }

            return Next.Execute(state);
        }

        private bool ShowHelp(State state)
        {
            var helpFlags = ParserSettings?.HelpFlag?.Names;
            if (helpFlags == null)
            {
                return false;
            }

            if (!state.TryExtractFlag(helpFlags, out _, out var newState))
            {
                return false;
            }

            return !newState.GetRemainingArguments(out newState).Any();
        }
    }
}
