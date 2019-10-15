namespace FluentArgs.Execution
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Extensions;
    using FluentArgs.Parser;
    using FluentArgs.Reflection;

    internal class ParameterStep : Step
    {
        public ParameterStep(Step previous, Parameter parameter)
            : base(previous)
        {
            this.Description = parameter;
        }

        public Parameter Description { get; }

        public override Task Accept(IStepVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override Task Execute(State state)
        {
            if (state.TryExtractNamedArgument(Description.Name.Names, out _, out var value, out var newState))
            {
                state = newState.AddParameter(
                    value!.TryParse(Description.Type, Description.Parser, Description.Name)
                        .ValidateIfRequired(Description.Validation, Description.Name));
            }
            else
            {
                if (Description.IsRequired)
                {
                    throw new ArgumentMissingException("Required parameter not found!", Description.Type, Description.Name);
                }

                if (Description.HasDefaultValue)
                {
                    state = state.AddParameter(Description.DefaultValue);
                }
                else
                {
                    state = state.AddParameter(Default.Instance(Description.Type));
                }
            }

            return GetNextStep().Execute(state);
        }
    }
}
