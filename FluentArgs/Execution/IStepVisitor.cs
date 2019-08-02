using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentArgs.Execution
{
    internal interface IStepVisitor
    {
        Task Visit(CallStep step);

        Task Visit(UntypedCallStep step);

        Task Visit(FlagStep step);

        Task Visit(GivenCommandStep step);

        Task Visit(GivenFlagStep step);

        Task Visit(GivenParameterStep step);

        Task Visit(InitialStep step);

        Task Visit(InvalidStep step);

        Task Visit(ParameterListStep step);

        Task Visit(ParameterStep step);

        Task Visit(RemainingArgumentsStep step);
    }
}
