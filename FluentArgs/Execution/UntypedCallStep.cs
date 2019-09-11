namespace FluentArgs.Execution
{
    using System;
    using System.Threading.Tasks;
    using FluentArgs.Description;

    internal class UntypedCallStep : Step
    {
        private readonly UntypedTargetFunction targetFunction;

        public UntypedCallStep(Step previous, UntypedTargetFunction targetFunction)
            : base(previous)
        {
            this.targetFunction = targetFunction;
        }

        public override Task Accept(IStepVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override Task Execute(State state)
        {
            var arguments = state.GetParameters();

            if (targetFunction.AsyncTarget != null)
            {
                return targetFunction.AsyncTarget(arguments);
            }

            targetFunction.Target(arguments);
            return Task.CompletedTask;
        }
    }
}
