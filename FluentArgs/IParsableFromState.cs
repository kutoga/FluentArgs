namespace FluentArgs
{
    using System.Threading.Tasks;
    using FluentArgs.Execution;

    internal interface IParsableFromState : IParsable
    {
        Task<bool> ParseFromState(State state);
    }
}
