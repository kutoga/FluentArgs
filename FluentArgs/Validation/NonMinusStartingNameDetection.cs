using FluentArgs.Description;
using FluentArgs.Execution;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentArgs.Validation
{
    internal class NonMinusStartingNameDetection : IStepVisitor
    {
        public Task Visit(CallStep step)
        {
            throw new NotImplementedException();
        }

        public Task Visit(UntypedCallStep step)
        {
            throw new NotImplementedException();
        }

        public Task Visit(FlagStep step)
        {
            throw new NotImplementedException();
        }

        public Task Visit(GivenCommandStep step)
        {
            throw new NotImplementedException();
        }

        public Task Visit(GivenFlagStep step)
        {
            throw new NotImplementedException();
        }

        public Task Visit(GivenParameterStep step)
        {
            throw new NotImplementedException();
        }

        public Task Visit(InitialStep step)
        {
            throw new NotImplementedException();
        }

        public Task Visit(InvalidStep step)
        {
            throw new NotImplementedException();
        }

        public Task Visit(ParameterListStep step)
        {
            throw new NotImplementedException();
        }

        public Task Visit(ParameterStep step)
        {
            throw new NotImplementedException();
        }

        public Task Visit(PositionalArgumentStep step)
        {
            throw new NotImplementedException();
        }

        public Task Visit(RemainingArgumentsStep step)
        {
            throw new NotImplementedException();
        }

        private static void ValidateAliases(IEnumerable<string> aliases)
        {
            var nonMinusStartingAliases = aliases.Where(StartsNotWithMinus).ToArray();
            if (nonMinusStartingAliases.Any())
            {
                throw new Exception("The")
            }

            bool StartsNotWithMinus(string alias) => !alias.StartsWith("-", CultureInfo.InvariantCulture);
        }
    }
}
