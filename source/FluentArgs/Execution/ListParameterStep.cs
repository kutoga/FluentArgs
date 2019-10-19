namespace FluentArgs.Execution
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Exceptions;
    using FluentArgs.Extensions;

    internal class ListParameterStep : Step
    {
        public ListParameterStep(Step previous, ListParameter listParameter)
            : base(previous)
        {
            Description = listParameter;
        }

        public ListParameter Description { get; } // TODO: rename to "description" (everywhere)

        public override Task Accept(IStepVisitor visitor)
        {
            return visitor.Visit(this);
        }

        public override Task Execute(State state)
        {
            if (state.TryExtractNamedArgument(Description.Name.Names, out var argument, out var value, out var newState))
            {
                state = newState.AddParameter(Parse(value!));
            }
            else
            {
                if (Description.IsRequired)
                {
                    throw new ArgumentMissingException("Required (list-)parameter not found!", Description.Type, Description.Name);
                }

                if (Description.HasDefaultValue)
                {
                    state = state.AddParameter(Description.DefaultValue);
                }
                else
                {
                    state = state.AddParameter(null);
                }
            }

            return GetNextStep().Execute(state);
        }

        private object Parse(string parameter)
        {
            var splitParameters = parameter.Split(Description.Separators.ToArray(), StringSplitOptions.None);
            return Reflection.Array.Create(Description.Type, splitParameters
                .Select(a => a.TryParse(Description.Type, Description.Parser))
                .ValidateIfRequired(Description.Validation).ToArray());
        }
    }
}
