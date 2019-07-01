namespace FluentArgs.Execution
{
    using System;
    using System.Threading.Tasks;
    using FluentArgs.Description;

    internal class GivenCommandStep : Step
    {
        private readonly GivenCommandBranch branch;
        private readonly IParsableFromState thenStep;

        public GivenCommandStep(Step previousStep, GivenCommandBranch branch, IParsableFromState thenStep)
            : base(previousStep)
        {
            this.branch = branch;
            this.thenStep = thenStep;
        }

        public override Task Execute(State state)
        {
            switch (branch.Type)
            {
                case GivenCommandBranchType.HasValue:
                    return ExecuteHasValue(state);

                case GivenCommandBranchType.Matches:
                    return ExecuteMatches(state);

                case GivenCommandBranchType.Ignore:
                    return Next.Execute(state);

                case GivenCommandBranchType.Invalid:
                    throw new Exception("TODO");

                default:
                    throw new Exception("invalid type?!?");
            }
        }

        private Task ExecuteHasValue(State state)
        {
        }

        private Task ExecuteMatches(State state)
        {

        }
    }
}
