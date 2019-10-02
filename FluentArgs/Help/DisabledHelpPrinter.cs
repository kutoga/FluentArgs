using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentArgs.Help
{
    public class DisabledHelpPrinter : IHelpPrinter
    {
        private DisabledHelpPrinter()
        {
        }

        public static DisabledHelpPrinter Instance { get; } = new DisabledHelpPrinter();

        public new Task Finalize()
        {
            return Task.CompletedTask;
        }

        public Task WriteApplicationDescription(string description)
        {
            return Task.CompletedTask;
        }

        public Task WriteFlagInfos(IReadOnlyCollection<string> aliases, string description, IReadOnlyCollection<(IReadOnlyCollection<string> aliases, string description)> givenHints)
        {
            return Task.CompletedTask;
        }

        public Task WriteParameterInfos(IReadOnlyCollection<string> aliases, string description, Type type, bool optional, bool hasDefaultValue, object defaultValue, IReadOnlyCollection<string> examples, IReadOnlyCollection<(IReadOnlyCollection<string> aliases, string description)> givenHints)
        {
            return Task.CompletedTask;
        }

        public Task WriteParameterListInfos(IReadOnlyCollection<string> aliases, string description, Type type, bool optional, IReadOnlyCollection<string> separators, bool hasDefaultValue, object defaultValue, IReadOnlyCollection<string> examples, IReadOnlyCollection<(IReadOnlyCollection<string> aliases, string description)> givenHints)
        {
            return Task.CompletedTask;
        }

        public Task WritePositionalArgumentInfos(string description, Type type, bool optional, bool hasDefaultValue, object defaultValue, IReadOnlyCollection<string> examples, IReadOnlyCollection<(IReadOnlyCollection<string> aliases, string description)> givenHints)
        {
            return Task.CompletedTask;
        }

        public Task WriteRemainingArgumentsAreUsed(string description, Type type, IReadOnlyCollection<string> examples, IReadOnlyCollection<(IReadOnlyCollection<string> aliases, string description)> givenHints)
        {
            return Task.CompletedTask;
        }
    }
}
