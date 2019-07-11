namespace FluentArgs.Execution
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Parser;

    internal class RemainingArgumentsStep : Step
    {
        private readonly RemainingArguments remainingArguments;

        public RemainingArgumentsStep(Step previousStep, RemainingArguments remainingArguments)
            : base(previousStep)
        {
            this.remainingArguments = remainingArguments;
        }

        public override Task Execute(State state)
        {
            var remainingArguments = state.Arguments;
            var parameter = Reflection.Array.Create(this.remainingArguments.Type, remainingArguments.Select(a => Parse(a)).ToArray());

            state = state.AddParameter(parameter).RemoveAllArguments();
            return Next.Execute(state);
        }

        private object Parse(string parameter)
        {
            if (remainingArguments.Parser != null)
            {
                return remainingArguments.Parser(parameter);
            }

            if (DefaultStringParsers.TryGetParser(remainingArguments.Type, out var parser))
            {
                return parser!(parameter);
            }

            throw new NotImplementedException();
        }
    }
}
