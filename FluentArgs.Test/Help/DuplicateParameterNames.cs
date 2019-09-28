using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace FluentArgs.Test.Help
{
    public static class DuplicateParameterNames
    {
        [Fact]
        public static void GivenTwoIdenticalParameterNames_ShouldNotThrow()
        {
            var called = false;
            var builder = FluentArgsBuilder.New()
                .Parameter("-x").IsOptional()
                .Parameter("-x").IsOptional()
                .Call(_ => _ => called = true);

            var parseSuccess = builder.Parse();

            parseSuccess.Should().BeTrue();
            called.Should().BeTrue();
        }

        [Fact]
        public static void ParsingTwoIdenticalNamedParameters_ShouldWork()
        {
            var args = new[] { "-x", "a", "-x", "b" };
            string? x1Parsed = null;
            string? x2Parsed = null;
            var builder = FluentArgsBuilder.New()
                .Parameter("-x").IsOptional()
                .Parameter("-x").IsOptional()
                .Call(x2 => x1 =>
                {
                    x1Parsed = x1;
                    x2Parsed = x2;
                });

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            x1Parsed.Should().Be("a");
            x2Parsed.Should().Be("b");
        }

        [Fact]
        public static void ParsingTwoIdenticalNamedParameters_ShouldThrowIfConfigured()
        {
            var called = false;
            var builder = FluentArgsBuilder.New()
                .ThrowOnDuplicateNames()
                .Parameter("-x").IsOptional()
                .Parameter("-x").IsOptional()
                .Call(_ => _ => called = true);

            Action parseAction = () => builder.Parse();

            parseAction.Should().Throw<Exception>();
            called.Should().BeFalse();
        }

        [Fact]
        public static void ParsingTwoIdenticalNamedParametersWithMultipleAliases_ShouldThrowIfConfigured()
        {
            var called = false;
            var builder = FluentArgsBuilder.New()
                .ThrowOnDuplicateNames()
                .Parameter("-x1", "-x").IsOptional()
                .Parameter("-x", "-y").IsOptional()
                .Call(_ => _ => called = true);

            Action parseAction = () => builder.Parse();

            parseAction.Should().Throw<Exception>();
            called.Should().BeFalse();
        }

        [Fact]
        public static void ParsingAParameterAndAFlagWithIdenticalNames_ShouldThrowIfConfigured()
        {
            var called = false;
            var builder = FluentArgsBuilder.New()
                .ThrowOnDuplicateNames()
                .Parameter("-x").IsOptional()
                .Flag("-x")
                .Call(_ => _ => called = true);

            Action parseAction = () => builder.Parse();

            parseAction.Should().Throw<Exception>();
            called.Should().BeFalse();
        }

        [Fact]
        public static void ParsingAParameterAndACommandWithIdenticalNames_ShouldThrowIfConfigured()
        {
            var called = false;
            var builder = FluentArgsBuilder.New()
                .ThrowOnDuplicateNames()
                .Parameter("-x").IsOptional()
                .Given.Command("-x").HasValue(1).Then(_ => called = true).ElseIgnore()
                .Call(_ => called = true);

            Action parseAction = () => builder.Parse();

            parseAction.Should().Throw<Exception>();
            called.Should().BeFalse();
        }

        [Fact]
        public static void ParsingAFlagAndACommandWithIdenticalNames_ShouldThrowIfConfigured()
        {
            var called = false;
            var builder = FluentArgsBuilder.New()
                .ThrowOnDuplicateNames()
                .Flag("-x")
                .Given.Command("-x").HasValue(1).Then(_ => called = true).ElseIgnore()
                .Call(_ => called = true);

            Action parseAction = () => builder.Parse();

            parseAction.Should().Throw<Exception>();
            called.Should().BeFalse();
        }

        [Fact]
        public static void ParsingIdenticalNamesOnDifferentBranches_ShouldWork()
        {
            var called = false;
            var builder = FluentArgsBuilder.New()
                .ThrowOnDuplicateNames()
                .Given.Flag("-a1").Then(
                    b => b
                        .Parameter("-x").IsRequired()
                        .Call(_ => called = true))
                .Given.Flag("-a2").Then(
                    b => b
                        .Parameter("-x").IsRequired()
                        .Call(_ => called = true))
                .Call(() => called = true);

            bool parseSuccess = builder.Parse();

            parseSuccess.Should().BeTrue();
            called.Should().BeTrue();
        }

        [Fact]
        public static void IncludingTheSameParameterNameMultipleTimesInTheSameDefinition_ShouldAlwaysThrow()
        {
            var called = false;
            var builder = FluentArgsBuilder.New()
                .Parameter("-x", "-x").IsOptional()
                .Call(_ => called = true);

            Action parseAction = () => builder.Parse();

            parseAction.Should().Throw<Exception>();
            called.Should().BeFalse();
        }
    }
}
