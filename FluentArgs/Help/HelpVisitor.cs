namespace FluentArgs.Help
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Execution;
    using FluentArgs.Extensions;

    internal class HelpVisitor : IStepVisitor
    {
        private readonly IHelpPrinter helpPrinter;

        public HelpVisitor(IHelpPrinter helpPrinter)
        {
            this.helpPrinter = helpPrinter;
        }

        public Task Visit(CallStep step)
        {
            return Task.CompletedTask;
        }

        public Task Visit(FlagStep step)
        {
            return helpPrinter.WriteFlagInfos(step.Description.Name.Names, step.Description.Description);
        }

        public async Task Visit(GivenCommandStep step)
        {
            // TODO: push command info
            await step.Branches.Select(b =>
            {
                // Write info
                if (b.then is FluentArgsDefinition argsBuilder)
                {
                    return argsBuilder.InitialStep.Accept(this);
                }

                return Task.CompletedTask;
            }).Serialize().ConfigureAwait(false);
            // TODO: pop command info

            throw new NotImplementedException();
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
            return Task.CompletedTask;
        }

        public async Task Visit(ParameterListStep step)
        {
            var parameterList = step.Description;
            await helpPrinter.WriteParameterListInfos(
                parameterList.Name.Names,
                parameterList.Description,
                parameterList.Type,
                !parameterList.IsRequired,
                parameterList.Separators,
                parameterList.HasDefaultValue,
                parameterList.DefaultValue,
                parameterList.Examples).ConfigureAwait(false);
            await step.Next.Accept(this).ConfigureAwait(false);
        }

        public async Task Visit(ParameterStep step)
        {
            var parameter = step.Description;
            await helpPrinter.WriteParameterInfos(
                parameter.Name.Names,
                parameter.Description,
                parameter.Type,
                !parameter.IsRequired,
                parameter.HasDefaultValue,
                parameter.DefaultValue,
                parameter.Examples ?? Array.Empty<string>()).ConfigureAwait(false);
            await step.Next.Accept(this).ConfigureAwait(false);
        }

        public Task Visit(RemainingArgumentsStep step)
        {
            throw new System.NotImplementedException();
        }
    }
}
