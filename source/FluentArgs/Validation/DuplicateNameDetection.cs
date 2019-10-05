namespace FluentArgs.Validation
{
    using System;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Execution;

    internal class DuplicateNameDetection : IStepVisitor
    {
        private readonly IImmutableSet<string> registeredNames;

        public DuplicateNameDetection()
            : this(ImmutableHashSet<string>.Empty)
        {
        }

        private DuplicateNameDetection(IImmutableSet<string> registeredNames)
        {
            this.registeredNames = registeredNames;
        }

        public Task Visit(CallStep step)
        {
            return Task.CompletedTask;
        }

        public Task Visit(UntypedCallStep step)
        {
            return Task.CompletedTask;
        }

        public Task Visit(FlagStep step)
        {
            var newDuplicationDetection = ValidateName(step.Description.Name);
            return step.GetNextStep().Accept(newDuplicationDetection);
        }

        public Task Visit(GivenCommandStep step)
        {
            var newDuplicationDetection = ValidateName(step.Name);
            var branchTasks = Task.WhenAll(step.Branches.Select(b =>
            {
                if (b.then is FluentArgsDefinition argsBuilder)
                {
                    return argsBuilder.InitialStep.Accept(newDuplicationDetection);
                }

                return Task.CompletedTask;
            }));
            return Task.WhenAll(branchTasks, step.GetNextStep().Accept(newDuplicationDetection));
        }

        public Task Visit(GivenFlagStep step)
        {
            var newDuplicationDetection = ValidateName(step.Description.Name);
            var branchTask = Task.CompletedTask;
            if (step.ThenStep is FluentArgsDefinition argsBuilder)
            {
                branchTask = argsBuilder.InitialStep.Accept(newDuplicationDetection);
            }

            return Task.WhenAll(branchTask, step.GetNextStep().Accept(newDuplicationDetection));
        }

        public Task Visit(GivenParameterStep step)
        {
            if (!step.Description.RequireExactValue)
            {
                return step.GetNextStep().Accept(this);
            }

            var newDuplicationDetection = ValidateName(step.Description.Name);
            var branchTask = Task.CompletedTask;
            if (step.ThenStep is FluentArgsDefinition argsBuilder)
            {
                branchTask = argsBuilder.InitialStep.Accept(newDuplicationDetection);
            }

            return Task.WhenAll(branchTask, step.GetNextStep().Accept(newDuplicationDetection));
        }

        public Task Visit(InitialStep step)
        {
            return step.GetNextStep().Accept(this);
        }

        public Task Visit(InvalidStep step)
        {
            return Task.CompletedTask;
        }

        public Task Visit(ParameterListStep step)
        {
            var newDuplicationDetection = ValidateName(step.Description.Name);
            return step.GetNextStep().Accept(newDuplicationDetection);
        }

        public Task Visit(ParameterStep step)
        {
            var newDuplicationDetection = ValidateName(step.Description.Name);
            return step.GetNextStep().Accept(newDuplicationDetection);
        }

        public Task Visit(PositionalArgumentStep step)
        {
            return step.GetNextStep().Accept(this);
        }

        public Task Visit(RemainingArgumentsStep step)
        {
            return step.GetNextStep().Accept(this);
        }

        private DuplicateNameDetection ValidateName(Name name)
        {
            var registeredNames = this.registeredNames;
            foreach (var alias in name.Names)
            {
                if (registeredNames.Contains(alias))
                {
                    throw new Exception($"The alias '{alias}' is used multiple times!");
                }

                registeredNames = registeredNames.Add(alias);
            }

            return new DuplicateNameDetection(registeredNames);
        }
   }
}
