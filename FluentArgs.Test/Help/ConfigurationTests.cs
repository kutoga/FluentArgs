namespace FluentArgs.Test.Help
{
    using System;
    using System.IO;
    using FluentAssertions;
    using Xunit;

    public static class Configuration
    {
        [Fact]
        public static void GivenAHelpConfiguration_ShouldBePossibleToPrintHelp()
        {
            var args = new[] { "--help" };
            var dummyOutput = new MemoryStream();
            var builder = FluentArgsBuilder.New()
                .RegisterHelpFlag("--help")
                .RegisterOutputStreams(dummyOutput, dummyOutput)
                .Invalid();

            Action parseAction = () => builder.Parse(args);

            parseAction.Should().NotThrow<Exception>();
        }
    }
}
