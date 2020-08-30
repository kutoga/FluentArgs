namespace FluentArgs.Execution
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using FluentArgs.Exceptions;

    internal class InvalidStep : Step
    {
        public InvalidStep(Step previousStep)
            : base(previousStep)
        {
        }

        public override Task Accept(IStepVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override Task Execute(State state)
        {
            throw new InvalidStateException();
        }
    }
}
