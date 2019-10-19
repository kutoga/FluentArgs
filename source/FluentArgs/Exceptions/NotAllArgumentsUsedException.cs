namespace FluentArgs.Exceptions
{
    using System;
    using System.Collections.Generic;

    internal class NotAllArgumentsUsedException : Exception
    {
        public NotAllArgumentsUsedException(IReadOnlyCollection<string> unusedArguments)
        {
            UnusedArguments = unusedArguments;
        }

        public IReadOnlyCollection<string> UnusedArguments { get; }
    }
}
