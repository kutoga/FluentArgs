namespace FluentArgs.Help
{
    using System;
    using System.Threading.Tasks;
    using FluentArgs.Execution;

    internal class HelpVisitor : IStepVisitor
    {
        private readonly IHelpPrinter helpPrinter;

        public HelpVisitor(IHelpPrinter helpPrinter)
        {
            this.helpPrinter = helpPrinter;
        }

        public Task Visit(CallStep step)
        {
            throw new System.NotImplementedException();
        }

        public Task Visit(FlagStep step)
        {
            throw new System.NotImplementedException();
        }

        public Task Visit(GivenCommandStep step)
        {
            throw new System.NotImplementedException();
        }

        public Task Visit(GivenFlagStep step)
        {
            throw new System.NotImplementedException();
        }

        public Task Visit(GivenParameterStep step)
        {
            throw new System.NotImplementedException();
        }

        public async Task Visit(InitialStep step)
        {
            var applicationDescription = step.ParserSettings.ApplicationDescription;
            if (applicationDescription != null)
            {
                await helpPrinter.WriteApplicationDescription(applicationDescription).ConfigureAwait(false);
            }

            await step.Next.Accept(this).ConfigureAwait(false);
        }

        public Task Visit(InvalidStep step)
        {
            throw new System.NotImplementedException();
        }

        public async Task Visit(ParameterListStep step)
        {
            var parameterList = step.Description;
            await helpPrinter.WriteParameterListInfos(
                parameterList.Name.Names,
                parameterList.Description,
                !parameterList.IsRequired,
                parameterList.Separators.ToArray(),
                ...);
        }

        public async Task Visit(ParameterStep step)
        {
            var parameter = step.Description;
            await helpPrinter.WriteParameterInfos(
                parameter.Name.Names,
                parameter.Description,
                !parameter.IsRequired,
                parameter.HasDefaultValue,
                parameter.Examples ?? Array.Empty<object>()).ConfigureAwait(false);
            await step.Next.Accept(this).ConfigureAwait(false);
        }

        public Task Visit(RemainingArgumentsStep step)
        {
            throw new System.NotImplementedException();
        }
    }
}
