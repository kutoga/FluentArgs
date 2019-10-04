namespace FluentArgs.Test.Help
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentArgs.Test.Helpers;
    using FluentAssertions;
    using Xunit;

    public static class ExampleTests
    {
        [Fact]
        public static void GivenExamplesForParametersAndRequestingHelp_ShouldListExamples()
        {
            var helpPrinter = new ExamplesCollectingHelpPrinter();
            var builder = FluentArgsBuilder.New()
                .RegisterHelpPrinter(helpPrinter)
                .RegisterHelpFlag("-h")
                .Parameter("-n")
                    .WithExamples("exampleA", "exampleB")
                    .IsRequired()
                .Call(a => { });

            var parseSuccess = builder.Parse("-h");

            parseSuccess.Should().BeTrue();
            helpPrinter.Examples.Should().BeEquivalentWithSameOrdering("exampleA", "exampleB");
        }

        [Fact]
        public static void GivenExamplesForParameterListsAndRequestingHelp_ShouldListExamples()
        {
            var helpPrinter = new ExamplesCollectingHelpPrinter();
            var builder = FluentArgsBuilder.New()
                .RegisterHelpPrinter(helpPrinter)
                .RegisterHelpFlag("-h")
                .ParameterList("-n")
                    .WithExamples("exampleA", "exampleB")
                    .IsRequired()
                .Call(a => { });

            var parseSuccess = builder.Parse("-h");

            parseSuccess.Should().BeTrue();
            helpPrinter.Examples.Should().BeEquivalentWithSameOrdering("exampleA", "exampleB");
        }

        [Fact]
        public static void GivenExamplesForPositionalArgumentssAndRequestingHelp_ShouldListExamples()
        {
            var helpPrinter = new ExamplesCollectingHelpPrinter();
            var builder = FluentArgsBuilder.New()
                .RegisterHelpPrinter(helpPrinter)
                .RegisterHelpFlag("-h")
                .PositionalArgument()
                    .WithExamples("exampleA", "exampleB")
                    .IsRequired()
                .Call(a => { });

            var parseSuccess = builder.Parse("-h");

            parseSuccess.Should().BeTrue();
            helpPrinter.Examples.Should().BeEquivalentWithSameOrdering("exampleA", "exampleB");
        }

        [Fact]
        public static void GivenExamplesForRemainingArgumentssAndRequestingHelp_ShouldListExamples()
        {
            var helpPrinter = new ExamplesCollectingHelpPrinter();
            var builder = FluentArgsBuilder.New()
                .RegisterHelpPrinter(helpPrinter)
                .RegisterHelpFlag("-h")
                .LoadRemainingArguments()
                    .WithExamples("exampleA", "exampleB")
                .Call(a => { });

            var parseSuccess = builder.Parse("-h");

            parseSuccess.Should().BeTrue();
            helpPrinter.Examples.Should().BeEquivalentWithSameOrdering("exampleA", "exampleB");
        }

        private class ExamplesCollectingHelpPrinter : IHelpPrinter
        {
            public IReadOnlyCollection<string>? Examples { get; private set; }

            public Task Finalize()
            {
                return Task.CompletedTask;
            }

            public Task WriteApplicationDescription(string description)
            {
                return Task.CompletedTask;
            }

            public Task WriteFlagInfos(IReadOnlyCollection<string> aliases, string? description, IReadOnlyCollection<(IReadOnlyCollection<string> aliases, string description)> givenHints)
            {
                return Task.CompletedTask;
            }

            public Task WriteParameterInfos(IReadOnlyCollection<string> aliases, string? description, Type type, bool optional, bool hasDefaultValue, object? defaultValue, IReadOnlyCollection<string> examples, IReadOnlyCollection<(IReadOnlyCollection<string> aliases, string description)> givenHints)
            {
                Examples = examples;
                return Task.CompletedTask;
            }

            public Task WriteParameterListInfos(IReadOnlyCollection<string> aliases, string? description, Type type, bool optional, IReadOnlyCollection<string> separators, bool hasDefaultValue, object? defaultValue, IReadOnlyCollection<string> examples, IReadOnlyCollection<(IReadOnlyCollection<string> aliases, string description)> givenHints)
            {
                Examples = examples;
                return Task.CompletedTask;
            }

            public Task WritePositionalArgumentInfos(string? description, Type type, bool optional, bool hasDefaultValue, object? defaultValue, IReadOnlyCollection<string> examples, IReadOnlyCollection<(IReadOnlyCollection<string> aliases, string description)> givenHints)
            {
                Examples = examples;
                return Task.CompletedTask;
            }

            public Task WriteRemainingArgumentsAreUsed(string? description, Type type, IReadOnlyCollection<string> examples, IReadOnlyCollection<(IReadOnlyCollection<string> aliases, string description)> givenHints)
            {
                Examples = examples;
                return Task.CompletedTask;
            }
        }
    }
}
