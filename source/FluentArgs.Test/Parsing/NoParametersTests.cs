namespace FluentArgs.Test.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
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

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            done.Should().BeTrue();
        }

        [Fact]
        public static void GivenNoArgumentsButParameters_ShouldBeParsable()
        {
            var done = false;
            var args = new[] { "-a", "-b", "--bla" };
            var builder = FluentArgsBuilder.New()
                .Call(() => done = true);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            done.Should().BeTrue();
        }

        [Fact]
        public static void GivenNoArguments_UntypedCallsShouldBePossible()
        {
            var args = Array.Empty<string>();
            IReadOnlyCollection<object?>? parameters = null;
            var builder = FluentArgsBuilder.New()
                .CallUntyped(p => parameters = p);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parameters.Should().BeEmpty();
        }
    }
}
