namespace FluentArgs.Help
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Execution;
    using FluentArgs.Extensions;

    internal class HelpVisitor : IStepVisitor
    {
        private readonly IHelpPrinter helpPrinter;
        private Stack<(Name name, string description)> givenTexts = new Stack<(Name name, string description)>();

        private IReadOnlyCollection<(IReadOnlyCollection<string> aliases, string description)> GetGivenHints()
        {
            return givenTexts.Select(t => ((IReadOnlyCollection<string>)t.name.Names, t.description)).ToArray();
        }

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
            return helpPrinter.WriteFlagInfos(step.Description.Name.Names, step.Description.Description, GetGivenHints());
        }

        public async Task Visit(GivenCommandStep step)
        {
            if (step.Branches.Last().branch.Type == GivenCommandBranchType.Invalid)
            {
                // TODO: The command is required; print this somehow
            }

            // TODO: push command info
            await step.Branches.Select(async b =>
            {
                switch (b.branch.Type)
                {
                    case GivenCommandBranchType.HasValue:
                        if (b.branch.PossibleValues.Length == 0)
                        {
                            givenTexts.Push((step.Name, "has a non-existing value"));
                        }
                        else if (b.branch.PossibleValues.Length == 1)
                        {
                            givenTexts.Push((step.Name, $" is {b.branch.PossibleValues[0]}"));
                        }
                        else
                        {
                            givenTexts.Push((step.Name, $" is one of the following values: {string.Join(", ", b.branch.PossibleValues)}"));
                        }

                        break;

                    case GivenCommandBranchType.Matches:
                        givenTexts.Push((step.Name, "matches a well-defined pattern"));
                        break;

                    case GivenCommandBranchType.Ignore:
                        break;
                }

                // Write info
                if (b.then is FluentArgsDefinition argsBuilder)
                {
                    await argsBuilder.InitialStep.Accept(this).ConfigureAwait(false);
                }

                givenTexts.Pop();
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
                parameterList.Examples,
                GetGivenHints()).ConfigureAwait(false);
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
                parameter.Examples ?? Array.Empty<string>(),
                GetGivenHints()).ConfigureAwait(false);
            await step.Next.Accept(this).ConfigureAwait(false);
        }

        public Task Visit(RemainingArgumentsStep step)
        {
            throw new System.NotImplementedException();
        }
    }
}
