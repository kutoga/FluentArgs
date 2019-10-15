namespace FluentArgs.Test.Helpers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class DummyParsingErrorPrinter :
        IParsingErrorPrinter
    {
        private readonly ConcurrentBag<(IReadOnlyCollection<string>? aliases, Type targetType, string description, IReadOnlyCollection<string>? helpFlagAliases)> argumentMissingErrors =
            new ConcurrentBag<(IReadOnlyCollection<string>? aliases, Type targetType, string description, IReadOnlyCollection<string>? helpFlagAliases)>();

        private readonly ConcurrentBag<(IReadOnlyCollection<string>? aliases, Type targetType, string description, IReadOnlyCollection<string>? helpFlagAliases)> argumentParsingErrors =
            new ConcurrentBag<(IReadOnlyCollection<string>? aliases, Type targetType, string description, IReadOnlyCollection<string>? helpFlagAliases)>();

        private readonly ConcurrentBag<(IReadOnlyCollection<string> aliases, string value, IReadOnlyCollection<string>? helpFlagAliases)> invalidCommandValueErrors =
            new ConcurrentBag<(IReadOnlyCollection<string> aliases, string value, IReadOnlyCollection<string>? helpFlagAliases)>();

        public IReadOnlyCollection<(IReadOnlyCollection<string>? aliases, Type targetType, string description, IReadOnlyCollection<string>? helpFlagAliases)> ArgumentMissingErrors =>
            argumentMissingErrors;

        public IReadOnlyCollection<(IReadOnlyCollection<string>? aliases, Type targetType, string description, IReadOnlyCollection<string>? helpFlagAliases)> ArgumentParsingErrors =>
            argumentParsingErrors;

        public IReadOnlyCollection<(IReadOnlyCollection<string> aliases, string value, IReadOnlyCollection<string>? helpFlagAliases)> InvalidCommandValueErrors =>
            invalidCommandValueErrors;

        public Task PrintArgumentMissingError(IReadOnlyCollection<string>? aliases, Type targetType, string description, IReadOnlyCollection<string>? helpFlagAliases)
        {
            argumentMissingErrors.Add((aliases, targetType, description, helpFlagAliases));
            return Task.CompletedTask;
        }

        public Task PrintArgumentParsingError(IReadOnlyCollection<string>? aliases, Type targetType, string description, IReadOnlyCollection<string>? helpFlagAliases)
        {
            argumentParsingErrors.Add((aliases, targetType, description, helpFlagAliases));
            return Task.CompletedTask;
        }

        public Task PrintInvalidCommandValueError(IReadOnlyCollection<string> aliases, string value, IReadOnlyCollection<string>? helpFlagAliases)
        {
            invalidCommandValueErrors.Add((aliases, value, helpFlagAliases));
            return Task.CompletedTask;
        }
    }
}
