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

            builder.Parse(args);

            done.Should().BeTrue();
        }

        [Fact]
        public static void GivenAnAsyncCall_TheTaskShouldBeForwarded()
        {
            Task dummyTask = Task.FromResult("My special task");
            var args = Array.Empty<string>();
            var builder = FluentArgsBuilder.New()
                .Call(() => dummyTask);

            var resultingTask = builder.ParseAsync(args);

            resultingTask.Should().Be(dummyTask);
        }

        [Fact]
        public static void GivenAnAsyncUntypedCall_TheTaskShouldBeForwarded()
        {
            var dummyTask = Task.FromResult("My special task");
            var args = Array.Empty<string>();
            var builder = FluentArgsBuilder.New()
                .CallUntyped(_ => dummyTask);

            var resultingTask = builder.ParseAsync(args);

            resultingTask.Should().Be(dummyTask);
        }

        [Fact]
        public static void GivenNoArgumentsButParameters_ShouldBeParsable()
        {
            var done = false;
            var args = new[] { "-a", "-b", "--bla" };
            var builder = FluentArgsBuilder.New()
                .Call(() => done = true);

            builder.Parse(args);

            done.Should().BeTrue();
        }

        [Fact]
        public static void GivenNoArguments_UntypedCallsShouldBePossible()
        {
            var args = Array.Empty<string>();
            IReadOnlyCollection<object?>? parameters = null;
            var builder = FluentArgsBuilder.New()
                .CallUntyped(p => parameters = p);

            builder.Parse(args);

            parameters.Should().BeEmpty();
        }
    }
}
