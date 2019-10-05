namespace FluentArgs.Description
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class UntypedTargetFunction
    {
        public UntypedTargetFunction(Action<IReadOnlyCollection<object?>> target)
        {
            Target = target;
        }

        public UntypedTargetFunction(Func<IReadOnlyCollection<object?>, Task> asyncTarget)
        {
            AsyncTarget = asyncTarget;
        }

        public Func<IReadOnlyCollection<object?>, Task>? AsyncTarget { get; }

        public Action<IReadOnlyCollection<object?>>? Target { get; }
    }
}
