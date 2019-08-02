namespace FluentArgs.Test.Parsing
{
    using FluentAssertions;
    using FluentArgs.Test.Helpers;
    using System.Collections.Generic;
    using Xunit;
    using System.Threading.Tasks;

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
                .Parameter("-b").IsRequired()
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

        [Fact]
        public static void GivenMultipleParametersAndAnUntypedCall_AllParametersShouldBeForwarded()
        {
            var args = new[] { "-a", "1", "-b", "hey" };
            IReadOnlyCollection<object?>? parameters = null;
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("-a").IsRequired()
                .Parameter("-b").IsOptional()
                .Parameter("-c").IsOptional()
                .CallUntyped(p => parameters = p);

            builder.Parse(args);

            parameters.Should().BeEquivalentWithSameOrdering(1, "hey", null);
        }

        [Fact]
        public static void GivenMultipleParametersAndAnAsyncUntypedCall_TheTaskShouldBeForwarded()
        {
            var dummyTask = Task.FromResult("My special task");
            var args = new[] { "-a", "1", "-b", "hey" };
            IReadOnlyCollection<object?>? parameters = null;
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("-a").IsRequired()
                .Parameter("-b").IsOptional()
                .Parameter("-c").IsOptional()
                .CallUntyped(_ => dummyTask);

            var resultingTask = builder.ParseAsync(args);

            resultingTask.Should().Be(dummyTask);
        }
    }
}
