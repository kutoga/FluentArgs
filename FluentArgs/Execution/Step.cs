namespace FluentArgs.Execution
{
    using System;
    using System.Threading.Tasks;

    internal abstract class Step
    {
        protected Step(Step previous)
        {
            if (previous.Next != null)
            {
                //TODO: better error
                throw new Exception("It is not possible to append two different steps to a given step!");
            }

            previous.Next = this;
            Previous = previous;
        }

        protected Step()
        {
        }

        public Step? Previous { get; private set; }

        public Step? Next { get; private set; }

        public abstract Task Execute(State state);

        public abstract Task Accept(IStepVisitor visitor);
    }
}
