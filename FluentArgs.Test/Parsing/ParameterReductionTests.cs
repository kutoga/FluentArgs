namespace FluentArgs.Test.Parsing
{
    using FluentAssertions;
    using Xunit;

    public static class ParameterReductionTests
    {
        [Fact]
        public static void IfAParameterIsUsedAndRemoved_TheSurroundingElementsShouldNotProduceANewParameter()
        {
            var args = new[] { "--a", "--b", "b", "a" };
            var builder = FluentArgsBuilder.New()
                .Parameter("--a").IsRequired()
                .Parameter("--b").IsRequired()
                .Call(b => a => { });

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeFalse();
        }

        [Fact]
        public static void IfAParameterIsUsedAndRemoved_TheSurroundingElementsShouldNotProduceANewParsableParameter()
        {
            var args = new[] { "--b", "--a", "a", "b" };
            string? parsedA = default;
            string? parsedB = default;
            var builder = FluentArgsBuilder.New()
                .Parameter("--a").IsOptional()
                .Parameter("--b").IsOptional()
                .Call(b => a =>
                {
                    parsedA = a;
                    parsedB = b;
                });

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedA.Should().Be("a");
            parsedB.Should().BeNull();
        }
    }
}
