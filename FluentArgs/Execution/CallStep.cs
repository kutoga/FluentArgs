namespace FluentArgs.Execution
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;

    internal class CallStep : Step
    {
        private TargetFunction targetFunction;

        public CallStep(Step previous, TargetFunction targetFunction)
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
            var result = Reflection.Method.InvokeWrappedMethod(targetFunction.Target, state.GetParameters(), true);
            if (result is null)
            {
                return Task.CompletedTask;
            }

            if (result is Task task)
            {
                return task;
            }

            throw new Exception("TODO: invalid result type");
        }
    }
}
