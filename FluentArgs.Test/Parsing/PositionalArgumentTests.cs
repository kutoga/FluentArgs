using FluentAssertions;
using Xunit;

namespace FluentArgs.Test.Parsing
{
    public static class PositionalArgumentTests
    {
        [Fact]
        public static void PositionalArgument_ShouldReturnTheNextUnusedArgument()
        {
            var args = new[] {"--timeout", "12", "/dev/stdout"};
            string? positionalArg = null;
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("--timeout").IsRequired()
                .PositionalArgument().IsRequired()
                .Call(arg => timeout => positionalArg = arg);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            positionalArg.Should().Be("/dev/stdout");
        }

        [Fact]
        public static void PositionalRequiredArgumentIfThereAreNoMoreArguments_ShouldNotBeSuccessful()
        {
            var args = new[] { "--timeout", "12" };
            string? positionalArg = null;
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("--timeout").IsRequired()
                .PositionalArgument().IsRequired()
                .Call(arg => timeout => positionalArg = arg);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeFalse();
            positionalArg.Should().Be(null);
        }

        [Fact]
        public static void PositionalOptionalArgumentIfThereAreNoMoreArguments_ShouldNotBeSuccessful()
        {
            var args = new[] { "--timeout", "12" };
            string? positionalArg = null;
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("--timeout").IsRequired()
                .PositionalArgument().IsOptional()
                .Call(arg => timeout => positionalArg = arg);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            positionalArg.Should().Be(null);
        }

        [Fact]
        public static void PositionalIntArgument_ShouldWork()
        {
            var args = new[] { "--timeout", "12", "42" };
            int? positionalArg = null;
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("--timeout").IsRequired()
                .PostionalArgument<int>().IsRequired()
                .Call(arg => timeout => positionalArg = arg);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            positionalArg.Should().Be(42);
        }

        [Fact]
        public static void MultiplePositionalArguments_ShouldWork()
        {
            var args = new[] { "a", "b", "c" };
            string? parsedA1 = null;
            string? parsedA2 = null;
            string? parsedA3 = null;
            var builder = FluentArgsBuilder.New()
                .PositionalArgument().IsRequired()
                .PositionalArgument().IsRequired()
                .PositionalArgument().IsRequired()
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

        [Fact]
        public static void GivenAValidatorAndRequiredInvalidInput_ShouldNotBeParsable()
        {
            var args = new[] { "110" };
            int? parsedN = null;
            var builder = FluentArgsBuilder.New()
                .PositionalArgument<int>()
                    .WithValidator(n => n >= 0 && n <= 100)
                    .IsRequired()
                .Call(n => parsedN = n);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeFalse();
            parsedN.Should().BeNull();
        }

        [Fact]
        public static void GivenAValidatorAndOptionalInvalidInput_ShouldNotBeParsable()
        {
            var args = new[] { "110" };
            int? parsedN = null;
            var builder = FluentArgsBuilder.New()
                .PositionalArgument<int>()
                    .WithValidator(n => n >= 0 && n <= 100)
                    .IsOptional()
                .Call(n => parsedN = n);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeFalse();
            parsedN.Should().BeNull();
        }
    }
}
