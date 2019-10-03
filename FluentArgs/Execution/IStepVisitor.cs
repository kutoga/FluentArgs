namespace FluentArgs.Execution
{
    using System.Threading.Tasks;

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

        Task Visit(PositionalArgumentStep step);

        Task Visit(RemainingArgumentsStep step);
    }
}
