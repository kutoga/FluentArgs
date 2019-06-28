namespace FluentArgs.Test
{
    using System;
    using FluentAssertions;
    using Xunit;

    public static class NoParametersTests
    {
        [Fact]
        public static void GivenNoArgumentsAndNoParameters_ShouldBeParsable()
        {
            var done = false;
            var args = Array.Empty<string>();
            var builder = FluentArgsBuilder.New()
                .Call(() => done = true);

            builder.Parse(args);

            done.Should().BeTrue();
        }
    }
}
