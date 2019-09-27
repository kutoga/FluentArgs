namespace FluentArgs.Test.Help
{
    using System;
    using System.IO;
    using FluentArgs.Help;
    using FluentAssertions;
    using Xunit;

    public static class Configuration
    {
        [Fact]
        public static void GivenAHelpConfiguration_ShouldBePossibleToPrintHelp()
        {
            var args = new[] { "--help" };
            var dummyOutput = new MemoryStream();
            var textOutput = new StreamWriter(dummyOutput);
            var called = false;
            var parseSuccess = false;
            var builder = FluentArgsBuilder.New()
                .RegisterHelpFlag("--help")
                .RegisterHelpPrinter(new SimpleHelpPrinter(textOutput))
                .Parameter("myParameter").IsRequired()
                .Call(_ => called = true);

            using (textOutput)
            {
                parseSuccess = builder.Parse(args);
            }

            parseSuccess.Should().BeTrue();
            called.Should().BeFalse();
            dummyOutput.ToArray().Should().NotBeEmpty();
        }

        [Fact]
        public static void GivenAllAssignmentOperatorsAreDisabled_ShouldNotUseThem()
        {
            var args = new[] { "-a=1" };
            var builder = FluentArgsBuilder.New()
                .WithoutAssignmentOperators()
                .Parameter("-a").IsRequired()
                .Call(_ => { });

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeFalse();
        }
    }
}
