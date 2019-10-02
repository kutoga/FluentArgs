using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace FluentArgs.Test.Help
{
    public static class UnusedArgumentsTests
    {
        [Fact]
        public static void UnusedArguments_ShouldBeIgnored()
        {
            var args = new[] {"-a", "-b"};
            var called = false;
            var builder = FluentArgsBuilder.New()
                .Call(() => called = true);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            called.Should().BeTrue();
        }

        [Fact]
        public static void UnusedArgumentsGivenTheRightConfig_ShouldThrow()
        {
            var args = new[] { "-a", "-b" };
            var builder = FluentArgsBuilder.New()
                .ThrowIfUnusedArgumentsArePresent()
                .Call(() => { });

            Action parseAction = () => builder.Parse(args);

            parseAction.Should().Throw<Exception>();
        }

        [Fact]
        public static void UnusedArgumentsGivenTheRightConfigAndAnUntypedCall_ShouldThrow()
        {
            var args = new[] { "-a", "-b" };
            var builder = FluentArgsBuilder.New()
                .ThrowIfUnusedArgumentsArePresent()
                .CallUntyped(_ => { });

            Action parseAction = () => builder.Parse(args);

            parseAction.Should().Throw<Exception>();
        }
    }
}
