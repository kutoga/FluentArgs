using FluentAssertions;
using Xunit;

namespace FluentArgs.Test.Parsing
{
    public static class PopArgumentsTests
    {
        [Fact]
        public static void PopArgument_ShouldReturnTheNextUnusedArgument()
        {
            var args = new[] {"--timeout", "12", "/dev/stdout"};
            string? poppedArg = null;
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("--timeout").IsRequired()
                .PopArgument().IsRequired()
                .Call(arg => timeout => poppedArg = arg);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            poppedArg.Should().Be("/dev/stdout");
        }

        [Fact]
        public static void PopRequiredArgumentIfThereAreNoMoreArguments_ShouldNotBeSuccessful()
        {
            var args = new[] { "--timeout", "12" };
            string? poppedArg = null;
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("--timeout").IsRequired()
                .PopArgument().IsRequired()
                .Call(arg => timeout => poppedArg = arg);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeFalse();
            poppedArg.Should().Be(null);
        }

        [Fact]
        public static void PopOptionalArgumentIfThereAreNoMoreArguments_ShouldNotBeSuccessful()
        {
            var args = new[] { "--timeout", "12" };
            string? poppedArg = null;
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("--timeout").IsRequired()
                .PopArgument().IsOptional()
                .Call(arg => timeout => poppedArg = arg);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            poppedArg.Should().Be(null);
        }

        [Fact]
        public static void PopIntArgument_ShouldWork()
        {
            var args = new[] { "--timeout", "12", "42" };
            int? poppedArg = null;
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("--timeout").IsRequired()
                .PopArgument<int>().IsRequired()
                .Call(arg => timeout => poppedArg = arg);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            poppedArg.Should().Be(42);
        }

        [Fact]
        public static void PoppingMultipleArguments_ShouldWork()
        {
            var args = new[] { "a", "b", "c" };
            string? parsedA1 = null;
            string? parsedA2 = null;
            string? parsedA3 = null;
            var builder = FluentArgsBuilder.New()
                .PopArgument().IsRequired()
                .PopArgument().IsRequired()
                .PopArgument().IsRequired()
                .Call(a3 => a2 => a1 =>
                {
                    parsedA1 = a1;
                    parsedA2 = a2;
                    parsedA3 = a3;
                });

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedA1.Should().Be("a");
            parsedA2.Should().Be("b");
            parsedA3.Should().Be("c");
        }
    }
}
