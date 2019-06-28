namespace FluentArgs.Execution
{
    using System.Threading.Tasks;

    internal class InitialStep : Step
    {
        public override Task Execute(State state)
        {
            if (Next == null)
            {
                return Task.CompletedTask;
            }

            return Next.Execute(state);
        }
    }
}
