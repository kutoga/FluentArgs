namespace FluentArgs.Test.Parsing
{
    using System;
    using System.Linq;
    using FluentArgs.Test.Helpers;
    using FluentAssertions;
    using Xunit;

    public static class ErrorTests
    {
        [Fact]
        public static void IfAnArgumentIsMissing_ShouldFailAndPropagateError()
        {
            var dummyParsingErrorPrinter = new DummyParsingErrorPrinter();
            var called = false;
            var builder = FluentArgsBuilder.New()
                .RegisterParsingErrorPrinter(dummyParsingErrorPrinter)
                .Parameter("-x").IsRequired()
                .Call(_ => called = true);

            var parseSuccess = builder.Parse(Array.Empty<string>());

            parseSuccess.Should().BeFalse();
            called.Should().BeFalse();
            dummyParsingErrorPrinter.ArgumentParsingErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.InvalidStateErrors.Should().Be(0);
            dummyParsingErrorPrinter.ArgumentMissingErrors.Count.Should().Be(1);
            dummyParsingErrorPrinter.ArgumentMissingErrors.First().aliases.Should().BeEquivalentTo("-x");
            dummyParsingErrorPrinter.ArgumentMissingErrors.First().helpFlagAliases.Should().BeNull();
            dummyParsingErrorPrinter.InvalidCommandValueErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.NotAllArgumentsAreUsedErrors.Should().BeEmpty();
        }

        [Fact]
        public static void IfAnArgumentIsMissingAndHelpFlagAvailable_ShouldRecommendHelp()
        {
            var dummyParsingErrorPrinter = new DummyParsingErrorPrinter();
            var builder = FluentArgsBuilder.New()
                .RegisterHelpFlag("-h", "--help")
                .RegisterParsingErrorPrinter(dummyParsingErrorPrinter)
                .Parameter("-x").IsRequired()
                .Call(_ => { });

            var parseSuccess = builder.Parse(Array.Empty<string>());

            parseSuccess.Should().BeFalse();
            dummyParsingErrorPrinter.ArgumentMissingErrors.First().helpFlagAliases.Should().BeEquivalentTo("-h", "--help");
        }

        [Fact]
        public static void IfArgumentCannotBeParsed_ShouldFailAndPropagateError()
        {
            var args = new[] { "-n", "ARGUS" };
            var dummyParsingErrorPrinter = new DummyParsingErrorPrinter();
            var called = false;
            var builder = FluentArgsBuilder.New()
                .RegisterParsingErrorPrinter(dummyParsingErrorPrinter)
                .Parameter<int>("-n").IsRequired()
                .Call(_ => called = true);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeFalse();
            called.Should().BeFalse();
            dummyParsingErrorPrinter.ArgumentMissingErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.InvalidStateErrors.Should().Be(0);
            dummyParsingErrorPrinter.ArgumentParsingErrors.Count.Should().Be(1);
            dummyParsingErrorPrinter.ArgumentParsingErrors.First().aliases.Should().BeEquivalentTo("-n");
            dummyParsingErrorPrinter.InvalidCommandValueErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.NotAllArgumentsAreUsedErrors.Should().BeEmpty();
        }

        [Fact]
        public static void IfArgumentWithACustomParserCannotBeParsed_ShouldFailAndPropagateError()
        {
            var args = new[] { "-n", "ARGUS" };
            var dummyParsingErrorPrinter = new DummyParsingErrorPrinter();
            var called = false;
            var builder = FluentArgsBuilder.New()
                .RegisterParsingErrorPrinter(dummyParsingErrorPrinter)
                .Parameter<int>("-n")
                    .WithParser(_ => throw new FormatException("I parse nothing!"))
                    .IsRequired()
                .Call(_ => called = true);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeFalse();
            called.Should().BeFalse();
            dummyParsingErrorPrinter.ArgumentMissingErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.InvalidStateErrors.Should().Be(0);
            dummyParsingErrorPrinter.ArgumentParsingErrors.Count.Should().Be(1);
            dummyParsingErrorPrinter.ArgumentParsingErrors.First().aliases.Should().BeEquivalentTo("-n");
            dummyParsingErrorPrinter.InvalidCommandValueErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.NotAllArgumentsAreUsedErrors.Should().BeEmpty();
        }

        [Fact]
        public static void IfArgumentCannotBeParsedAndHelpFlagAvailable_ShouldRecommendHelp()
        {
            var args = new[] { "-n", "ARGUS" };
            var dummyParsingErrorPrinter = new DummyParsingErrorPrinter();
            var builder = FluentArgsBuilder.New()
                .RegisterHelpFlag("-h", "--help")
                .RegisterParsingErrorPrinter(dummyParsingErrorPrinter)
                .Parameter<int>("-n").IsRequired()
                .Call(_ => { });

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeFalse();
            dummyParsingErrorPrinter.ArgumentParsingErrors.First().helpFlagAliases.Should().BeEquivalentTo("-h", "--help");
        }

        [Fact]
        public static void FailedValidation_ShouldRecommendHelp()
        {
            var args = new[] { "101" };
            var dummyParsingErrorPrinter = new DummyParsingErrorPrinter();
            var called = false;
            var builder = FluentArgsBuilder.New()
                .DefaultConfigs()
                .RegisterParsingErrorPrinter(dummyParsingErrorPrinter)
                .PositionalArgument<int>()
                    .WithValidation(n => n < 100)
                    .IsRequired()
                .Call(_ => called = true);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeFalse();
            called.Should().BeFalse();
            dummyParsingErrorPrinter.ArgumentMissingErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.InvalidStateErrors.Should().Be(0);
            dummyParsingErrorPrinter.ArgumentParsingErrors.Count.Should().Be(1);
            dummyParsingErrorPrinter.ArgumentParsingErrors.First().aliases.Should().BeNull();
            dummyParsingErrorPrinter.ArgumentParsingErrors.First().helpFlagAliases.Should().BeEquivalentTo("-h", "--help");
            dummyParsingErrorPrinter.InvalidCommandValueErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.NotAllArgumentsAreUsedErrors.Should().BeEmpty();
        }

        [Fact]
        public static void FailedValidationWithCustomParser_ShouldRecommendHelp()
        {
            var args = new[] { "101" };
            var dummyParsingErrorPrinter = new DummyParsingErrorPrinter();
            var called = false;
            var builder = FluentArgsBuilder.New()
                .DefaultConfigs()
                .RegisterParsingErrorPrinter(dummyParsingErrorPrinter)
                .PositionalArgument<int>()
                    .WithParser(_ => 110)
                    .WithValidation(n => n < 100)
                    .IsRequired()
                .Call(_ => called = true);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeFalse();
            called.Should().BeFalse();
            dummyParsingErrorPrinter.ArgumentMissingErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.InvalidStateErrors.Should().Be(0);
            dummyParsingErrorPrinter.ArgumentParsingErrors.Count.Should().Be(1);
            dummyParsingErrorPrinter.ArgumentParsingErrors.First().aliases.Should().BeNull();
            dummyParsingErrorPrinter.ArgumentParsingErrors.First().helpFlagAliases.Should().BeEquivalentTo("-h", "--help");
            dummyParsingErrorPrinter.InvalidCommandValueErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.NotAllArgumentsAreUsedErrors.Should().BeEmpty();
        }

        [Fact]
        public static void InvalidCommandValues_ShouldRecommendHelp()
        {
            var args = new[] { "-n", "1" };
            var dummyParsingErrorPrinter = new DummyParsingErrorPrinter();
            var called = false;
            var builder = FluentArgsBuilder.New()
                .DefaultConfigs()
                .RegisterParsingErrorPrinter(dummyParsingErrorPrinter)
                .Given.Command("-n")
                    .HasValue("111").Then(() => called = true)
                    .ElseIsInvalid()
                .Call(() => called = true);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeFalse();
            called.Should().BeFalse();
            dummyParsingErrorPrinter.ArgumentMissingErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.ArgumentParsingErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.InvalidStateErrors.Should().Be(0);
            dummyParsingErrorPrinter.InvalidCommandValueErrors.Count.Should().Be(1);
            dummyParsingErrorPrinter.InvalidCommandValueErrors.First().aliases.Should().BeEquivalentTo("-n");
            dummyParsingErrorPrinter.InvalidCommandValueErrors.First().helpFlagAliases.Should().BeEquivalentTo("-h", "--help");
            dummyParsingErrorPrinter.NotAllArgumentsAreUsedErrors.Should().BeEmpty();
        }

        [Fact]
        public static void IfTooManyArgumentsAreGiven_ShouldBeParsable()
        {
            var args = new[] { "a", "b" };
            var called = false;
            var builder = FluentArgsBuilder.New()
                .Call(() => called = true);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            called.Should().BeTrue();
        }

        [Fact]
        public static void IfTooManyArgumentsAreGivenAndItisDisallows_ShouldRecommendHelp()
        {
            var args = new[] { "a", "b" };
            var dummyParsingErrorPrinter = new DummyParsingErrorPrinter();
            var called = false;
            var builder = FluentArgsBuilder.New()
                .RegisterDefaultHelpFlags()
                .RegisterParsingErrorPrinter(dummyParsingErrorPrinter)
                .DisallowUnusedArguments()
                .Call(() => called = true);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeFalse();
            called.Should().BeFalse();
            dummyParsingErrorPrinter.ArgumentMissingErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.ArgumentParsingErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.InvalidCommandValueErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.InvalidStateErrors.Should().Be(0);
            dummyParsingErrorPrinter.NotAllArgumentsAreUsedErrors.Count.Should().Be(1);
            dummyParsingErrorPrinter.NotAllArgumentsAreUsedErrors.First().remainingArguments.Should().BeEquivalentWithSameOrdering("a", "b");
            dummyParsingErrorPrinter.NotAllArgumentsAreUsedErrors.First().helpFlagAliases.Should().BeEquivalentTo("-h", "--help");
        }

        [Fact]
        public static void InvalidStates_ShouldRecommendHelp()
        {
            var args = new[] { "-a" };
            var dummyParsingErrorPrinter = new DummyParsingErrorPrinter();
            var builder = FluentArgsBuilder.New()
                .RegisterDefaultHelpFlags()
                .RegisterParsingErrorPrinter(dummyParsingErrorPrinter)
                .Invalid();

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeFalse();
            dummyParsingErrorPrinter.ArgumentMissingErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.ArgumentParsingErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.InvalidCommandValueErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.NotAllArgumentsAreUsedErrors.Should().BeEmpty();
            dummyParsingErrorPrinter.InvalidStateErrors.Should().Be(1);
        }
    }
}
