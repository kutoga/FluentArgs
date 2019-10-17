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

        public Step GetNextStep()
        {
            if (Next == null)
            {
                throw new Exception("Cannot access next step! It is undefined!");
            }

            return Next;
        }
    }
}
