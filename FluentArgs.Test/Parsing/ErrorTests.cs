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

            parseSuccess.Should().BeTrue();
            called.Should().BeFalse();
            dummyParsingErrorPrinter.ArgumentParsingErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.ArgumentMissingErrors.Count.Should().Be(1);
            dummyParsingErrorPrinter.ArgumentMissingErrors.First().aliases.Should().BeEquivalentTo("-x");
            dummyParsingErrorPrinter.ArgumentMissingErrors.First().helpFlagAliases.Should().BeNull();
        }

        [Fact]
        public static void IfArgumentCannotBeParsed_ShouldFailAndPropagateError()
        {
            true.Should().BeFalse();
        }
    }
}
