using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentArgs.Test.Helpers;
using FluentAssertions;
using Xunit;

namespace FluentArgs.Test.Parsing
{
    public static class ErrorTests
    {
        [Fact]
        public static void IfAnArgumentIsMissing_ShouldFailAndPropagateError()
        {
            var dummyParsingErrorPrinter = new DummyParsingErrorPrinter();
            var called = false;
            var builder = FluentArgsBuilder.New()
                .RegisterParsingErrorPrinter(dummyParsingErrorPrinter)
                .Parameter("-x").IsRequired()
                .Call(_ => called = true);

            var parseSuccess = builder.Parse(Array.Empty<string>());

            parseSuccess.Should().BeFalse();
            called.Should().BeFalse();
            dummyParsingErrorPrinter.ArgumentParsingErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.ArgumentMissingErrors.Count.Should().Be(1);
            dummyParsingErrorPrinter.ArgumentMissingErrors.First().aliases.Should().BeEquivalentTo("-x");
            dummyParsingErrorPrinter.ArgumentMissingErrors.First().helpFlagAliases.Should().BeNull();
        }

        [Fact]
        public static void IfAnArgumentIsMissingAndHelpFlagAvailable_ShouldRecommendHelp()
        {
            var dummyParsingErrorPrinter = new DummyParsingErrorPrinter();
            var builder = FluentArgsBuilder.New()
                .RegisterHelpFlag("-h", "--help")
                .RegisterParsingErrorPrinter(dummyParsingErrorPrinter)
                .Parameter("-x").IsRequired()
                .Call(_ => { });

            var parseSuccess = builder.Parse(Array.Empty<string>());

            parseSuccess.Should().BeFalse();
            dummyParsingErrorPrinter.ArgumentMissingErrors.First().helpFlagAliases.Should().BeEquivalentTo("-h", "--help");
        }

        [Fact]
        public static void IfArgumentCannotBeParsed_ShouldFailAndPropagateError()
        {
            var args = new[] {"-n", "ARGUS"};
            var dummyParsingErrorPrinter = new DummyParsingErrorPrinter();
            var called = false;
            var builder = FluentArgsBuilder.New()
                .RegisterParsingErrorPrinter(dummyParsingErrorPrinter)
                .Parameter<int>("-n").IsRequired()
                .Call(_ => called = true);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeFalse();
            called.Should().BeFalse();
            dummyParsingErrorPrinter.ArgumentMissingErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.ArgumentParsingErrors.Count.Should().Be(1);
            dummyParsingErrorPrinter.ArgumentParsingErrors.First().aliases.Should().BeEquivalentTo("-n");
        }

        [Fact]
        public static void IfArgumentCannotBeParsedAndHelpFlagAvailable_ShouldRecommendHelp()
        {
            var args = new[] { "-n", "ARGUS" };
            var dummyParsingErrorPrinter = new DummyParsingErrorPrinter();
            var builder = FluentArgsBuilder.New()
                .RegisterHelpFlag("-h", "--help")
                .RegisterParsingErrorPrinter(dummyParsingErrorPrinter)
                .Parameter<int>("-n").IsRequired()
                .Call(_ => { });

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeFalse();
            dummyParsingErrorPrinter.ArgumentParsingErrors.First().helpFlagAliases.Should().BeEquivalentTo("-h", "--help");
        }

    }
}
