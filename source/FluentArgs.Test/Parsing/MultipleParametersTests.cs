namespace FluentArgs.Test.Parsing
{
    using System.Collections.Generic;
    using FluentArgs.Test.Helpers;
    using FluentAssertions;
    using Xunit;

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

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
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

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parameters.Should().BeEquivalentWithSameOrdering(1, "hey", null);
        }
    }
}
