namespace FluentArgs.Test.Parsing.Given
{
    using FluentAssertions;
    using System.Threading.Tasks;
    using Xunit;

    public static class GivenThenExtensionsTests
    {
        [Fact]
        public static void GivenThenWithoutParameters_ShouldRedirect()
        {
            var args = new[] { "-f" };
            bool? redirected = default;
            var builder = FluentArgsBuilder.New()
                .Given.Flag("-f").Then(() => redirected = true)
                .Call(() => redirected = false);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            redirected.Should().BeTrue();
        }

        [Fact]
        public static void AsyncGivenThenWithoutParameters_ShouldRedirect()
        {
            var args = new[] { "-f" };
            bool? redirected = default;
            var builder = FluentArgsBuilder.New()
                .Given.Flag("-f").Then(async () => redirected = true)
                .Call(() => redirected = false);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            redirected.Should().BeTrue();
        }

        [Fact]
        public static void GivenThenWithOneParameter_ShouldRedirect()
        {
            var args = new[] { "-f", "-p1", "1" };
            bool? redirected = default;
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("-p1").IsRequired()
                .Given.Flag("-f").Then(p1 => redirected = true)
                .Call(p1 => redirected = false);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            redirected.Should().BeTrue();
        }

        [Fact]
        public static void AsyncGivenThenWithOneParameter_ShouldRedirect()
        {
            var args = new[] { "-f", "-p1", "1" };
            bool? redirected = default;
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("-p1").IsRequired()
                .Given.Flag("-f").Then(async p1 => redirected = true)
                .Call(p1 => redirected = false);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            redirected.Should().BeTrue();
        }

        [Fact]
        public static void AsyncGivenThenWithTwoParameters_ShouldRedirect()
        {
            var args = new[] { "-f", "-p1", "1", "-p2", "2" };
            bool? redirected = default;
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("-p1").IsRequired()
                .Parameter<double>("-p2").IsRequired()
                .Given.Flag("-f").Then(p2 => p1 =>
                {
                    redirected = true;
                    return Task.CompletedTask;
                })
                .Call(p2 => p1 => redirected = false);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            redirected.Should().BeTrue();
        }

        [Fact]
        public static void GivenThenWithTwoParameters_ShouldRedirect()
        {
            var args = new[] { "-f", "-p1", "1", "-p2", "2" };
            bool? redirected = default;
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("-p1").IsRequired()
                .Parameter<double>("-p2").IsRequired()
                .Given.Flag("-f").Then(p2 => p1 => redirected = true)
                .Call(p2 => p1 => redirected = false);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            redirected.Should().BeTrue();
        }
    }
}
