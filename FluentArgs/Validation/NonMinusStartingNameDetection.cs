namespace FluentArgs.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Execution;

    internal class NonMinusStartingNameDetection : IStepVisitor
    {
        public Task Visit(CallStep step)
        {
            return Task.CompletedTask;
        }

        public Task Visit(UntypedCallStep step)
        {
            return Task.CompletedTask;
        }

        public Task Visit(FlagStep step)
        {
            ValidateAliases(step.Description.Name.Names);
            return step.GetNextStep().Accept(this);
        }

        public Task Visit(GivenCommandStep step)
        {
            ValidateAliases(step.Name.Names);
            return step.GetNextStep().Accept(this);
        }

        public Task Visit(GivenFlagStep step)
        {
            ValidateAliases(step.Description.Name.Names);
            return step.GetNextStep().Accept(this);
        }

        public Task Visit(GivenParameterStep step)
        {
            ValidateAliases(step.Description.Name.Names);
            return step.GetNextStep().Accept(this);
        }

        public Task Visit(InitialStep step)
        {
            return step.GetNextStep().Accept(this);
        }

        public Task Visit(InvalidStep step)
        {
            return Task.CompletedTask;
        }

        public Task Visit(ParameterListStep step)
        {
            ValidateAliases(step.Description.Name.Names);
            return step.GetNextStep().Accept(this);
        }

        public Task Visit(ParameterStep step)
        {
            ValidateAliases(step.Description.Name.Names);
            return step.GetNextStep().Accept(this);
        }

        public Task Visit(PositionalArgumentStep step)
        {
            return step.GetNextStep().Accept(this);
        }

        public Task Visit(RemainingArgumentsStep step)
        {
            return step.GetNextStep().Accept(this);
        }

        private static void ValidateAliases(IEnumerable<string> aliases)
        {
            var nonMinusStartingAliases = aliases.Where(StartsNotWithMinus).ToArray();
            if (nonMinusStartingAliases.Any())
            {
                throw new Exception($"The following defined aliases do not start with a minus '-': {string.Join(", ", nonMinusStartingAliases.OrderBy(a => a))}");
            }

            bool StartsNotWithMinus(string alias) => !alias.StartsWith("-", StringComparison.InvariantCulture);
        }
    }
}
