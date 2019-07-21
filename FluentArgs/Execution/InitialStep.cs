namespace FluentArgs.Execution
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;

    internal class InitialStep : Step
    {
        public ParserSettings? ParserSettings { get; set; }

        public override Task Execute(State state)
        {
            if (ShowHelp(state))
            {
                throw new Exception("TODO: show help");
            }

            if (Next == null)
            {
                throw new Exception("TODO: Good message");
                //return Task.CompletedTask;
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

            if (!state.TryExtractArguments(helpFlags, out var _, out var newState))
            {
                return false;
            }

            return newState.GetRemainingArguments(out newState).Count() == 0;
        }
    }
}
