namespace FluentArgs.Test.Parsing
{
    using FluentAssertions;
    using Xunit;

    public static class FlagTests
    {
        [Fact]
        public static void GivenAFlag_ItShouldBeRecognized()
        {
            var args = new[] { "-x" };
            bool? parsedX = default;
            var builder = FluentArgsBuilder.New()
                .Flag("-x")
                .Call(x => parsedX = x);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedX.Should().BeTrue();
        }

        [Fact]
        public static void GivenANonPresentFlag_ShouldNotBeRecognized()
        {
            var args = new[] { "-y" };
            bool? parsedX = default;
            var builder = FluentArgsBuilder.New()
                .Flag("-x")
                .Call(x => parsedX = x);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedX.Should().BeFalse();
        }

        [Theory]
        [InlineData("-x", "-y", true, true)]
        [InlineData("-x", "XX", true, false)]
        [InlineData("XX", "-y", false, true)]
        [InlineData("XX", "CC", false, false)]
        public static void GivenMultipleFlags_ShouldBeHandledCorrect(string flag1, string flag2, bool xPresent, bool yPresent)
        {
            var args = new[] { flag1, flag2 };
            bool? parsedX = default;
            bool? parsedY = default;
            var builder = FluentArgsBuilder.New()
                .Flag("-x")
                .Flag("-y")
                .Call(y => x =>
                {
                    parsedX = x;
                    parsedY = y;
                });

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedX.Should().Be(xPresent);
            parsedY.Should().Be(yPresent);
        }
    }
}
