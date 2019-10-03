namespace FluentArgs.Test.Parsing
{
    using FluentAssertions;
    using Xunit;

    public static class BooleanParserTests
    {
        [Theory]
        [InlineData("true")]
        [InlineData("TrUE")]
        [InlineData("1")]
        [InlineData("y")]
        [InlineData("Y")]
        [InlineData("yes")]
        [InlineData("yeS")]
        public static void ParseTrue_ShouldWork(string argument)
        {
            var args = new[] { "-b", argument };
            bool? parsedB = default;
            var builder = FluentArgsBuilder.New()
                .Parameter<bool>("-b").IsRequired()
                .Call(b => parsedB = b);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedB.Should().BeTrue();
        }

        [Theory]
        [InlineData("false")]
        [InlineData("FALse")]
        [InlineData("0")]
        [InlineData("n")]
        [InlineData("N")]
        [InlineData("no")]
        [InlineData("NO")]
        public static void ParseFalse_ShouldWork(string argument)
        {
            var args = new[] { "-b", argument };
            bool? parsedB = default;
            var builder = FluentArgsBuilder.New()
                .Parameter<bool>("-b").IsRequired()
                .Call(b => parsedB = b);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedB.Should().BeFalse();
        }
    }
}
