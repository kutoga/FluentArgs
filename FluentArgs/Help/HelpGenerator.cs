namespace FluentArgs.Help
{
    using System.Threading.Tasks;
    using FluentArgs.Execution;

    internal class HelpGenerator
    {
        private readonly InitialStep initialStep;

        public HelpGenerator(InitialStep initialStep)
        {
            this.initialStep = initialStep;
        }

        public async Task Write()
        {
            var visitor = new HelpVisitor(initialStep.ParserSettings.HelpPrinter);
            await visitor.Visit(initialStep).ConfigureAwait(false);
            await initialStep.ParserSettings.HelpPrinter.Finalize().ConfigureAwait(false);
        }
    }
}
