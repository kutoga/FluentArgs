using FluentArgs.Description;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentArgs.Execution
{
    internal class FlagStep : Step
    {
        private readonly Flag flag;

        public FlagStep(Step previousStep, Flag flag)
            : base(previousStep)
        {
            this.flag = flag;
        }

        public override Task Execute(State state)
        {
            var flagIndex = state.Arguments
                .Select((a, i) => (argument: a, index: i))
                .Where(p => flag.Name.Names.Contains(p.argument))
                .Select(p => (int?)p.index)
                .FirstOrDefault();

            if (flagIndex == null)
            {
                state = state.AddParameter(false);
            }
            else
            {
                state = state
                    .RemoveArguments(flagIndex.Value)
                    .AddParameter(true);
            }

            return Next.Execute(state);
        }
    }
}
