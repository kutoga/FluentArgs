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

        public Task Write()
        {
            var visitor = new HelpVisitor(initialStep.ParserSettings.HelpPrinter);
            return visitor.Visit(initialStep);
        }
    }
}
