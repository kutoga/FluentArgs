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
            var builder = FluentArgsBuilder.New()
                .RegisterHelpFlag("--help")
                .RegisterHelpPrinter(new SimpleHelpPrinter(textOutput, textOutput))
                .Invalid();

            Action parseAction = () => builder.Parse(args);

            parseAction.Should().NotThrow<Exception>();
        }
    }
}
