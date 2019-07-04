namespace FluentArgs.Execution
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    internal class InvalidStep : Step
    {
        public InvalidStep(Step previousStep)
            : base(previousStep)
        {
        }

        public override Task Execute(State state)
        {
            throw new Exception("TODO: Invalid state: show helP");
        }
    }
}
