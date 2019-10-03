namespace FluentArgs.Test.Helpers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public class DummyParsingErrorPrinter : IParsingErrorPrinter
    {
        private readonly ConcurrentBag<(IReadOnlyCollection<string> aliases, string description, IReadOnlyCollection<string> helpFlagAliases)> argumentMissingErrors =
            new ConcurrentBag<(IReadOnlyCollection<string> aliases, string description, IReadOnlyCollection<string> helpFlagAliases)>();

        private readonly ConcurrentBag<(IReadOnlyCollection<string> aliases, string description, IReadOnlyCollection<string> helpFlagAliases)> argumentParsingErrors =
            new ConcurrentBag<(IReadOnlyCollection<string> aliases, string description, IReadOnlyCollection<string> helpFlagAliases)>();

        public IReadOnlyCollection<(IReadOnlyCollection<string> aliases, string description, IReadOnlyCollection<string> helpFlagAliases)> ArgumentMissingErrors =>
            argumentMissingErrors;

        public IReadOnlyCollection<(IReadOnlyCollection<string> aliases, string description, IReadOnlyCollection<string> helpFlagAliases)> ArgumentParsingErrors =>
            argumentParsingErrors;

        public Task PrintArgumentMissingError(IReadOnlyCollection<string> aliases, string description, IReadOnlyCollection<string> helpFlagAliases)
        {
            argumentMissingErrors.Add((aliases, description, helpFlagAliases));
            return Task.CompletedTask;
        }

        public Task PrintArgumentParsingError(IReadOnlyCollection<string> aliases, string description, IReadOnlyCollection<string> helpFlagAliases)
        {
            argumentParsingErrors.Add((aliases, description, helpFlagAliases));
            return Task.CompletedTask;
        }
    }
}
