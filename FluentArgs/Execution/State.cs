namespace FluentArgs.Execution
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;

    internal class State
    {
        private IImmutableList<object?> parameters;
        private IImmutableList<string> arguments;

        private State(IImmutableList<object?> parameters, IImmutableList<string> arguments)
        {
            this.parameters = parameters;
            this.arguments = arguments;
        }

        public IReadOnlyList<string> Arguments => arguments;

        public static State InitialState(IEnumerable<string> arguments)
        {
            return new State(ImmutableList<object?>.Empty, arguments.ToImmutableList());
        }

        public State AddParameter(object? parameter)
        {
            return new State(parameters.Add(parameter), arguments);
        }

        public State RemoveArguments(int index, params int[] moreIndices)
        {
            //TODO: when arguments are removed, then the simple join of the other arguments is invalid!
            // -> fix
            var arguments = this.arguments;
            var indicesDesc = moreIndices
                .Concat(new[] { index })
                .OrderByDescending(i => i);

            foreach (var i in indicesDesc)
            {
                arguments = arguments.RemoveAt(i);
            }

            return new State(parameters, arguments);
        }

        public IReadOnlyList<object?> GetParameters()
        {
            return parameters;
        }
    }
}
