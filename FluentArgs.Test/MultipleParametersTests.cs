namespace FluentArgs.Test
{
    using FluentAssertions;
    using Xunit;

    public static class MultipleParametersTests
    {
        [Fact]
        public static void GivenMultipleParametersAndMultipleArguments_ParsingShouldWork()
        {
            long? parsedA = default;
            string? parsedB = default;
            bool? parsedC = default;
            var args = new[] { "-a", "1337", "-b", "beni", "-c", "true" };
            var builder = FluentArgsBuilder.New()
                .Parameter<long>("-a").IsRequired()
                .Parameter<string>("-b").IsRequired()
                .Parameter<bool>("-c").IsRequired()
                .Call(c => b => a =>
                {
                    parsedA = a;
                    parsedB = b;
                    parsedC = c;
                });

            builder.Parse(args);

            parsedA.Should().Be(1337);
            parsedB.Should().Be("beni");
            parsedC.Should().BeTrue();
        }
    }
}
