namespace FluentArgs.Test.Parsing
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentArgs.Test.Helpers;
    using FluentAssertions;
    using Xunit;

    public static class RemainingArgumentsTests
    {
        [Fact]
        public static void GivenNoParametersAreDefined_AllParametersShouldBeRemainingArguments()
        {
            var args = new[] { "-x", "a", "-y", "b" };
            IReadOnlyList<string>? parsedArgs = default;
            var builder = FluentArgsBuilder.New()
                .LoadRemainingArguments()
                .Call(args => parsedArgs = args);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedArgs.Should().BeEquivalentWithSameOrdering(new[] { "-x", "a", "-y", "b" });
        }

        [Fact]
        public static void GivenNoParametersAreDefined_AllParametersShouldBeRemainingArgumentsWhenCalledAsync()
        {
            var args = new[] { "-x", "a", "-y", "b" };
            IReadOnlyList<string>? parsedArgs = default;
            var builder = FluentArgsBuilder.New()
                .LoadRemainingArguments()
                .Call(async args => parsedArgs = args);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedArgs.Should().BeEquivalentWithSameOrdering(new[] { "-x", "a", "-y", "b" });
        }

        [Fact]
        public static void GivenSomeParametersAreDefined_TheUnsedParametersShouldBeAvailable()
        {
            var args = new[] { "a", "-x", "1", "b", "c" };
            IReadOnlyList<string>? parsedArgs = default;
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("-x").IsRequired()
                .LoadRemainingArguments()
                .Call(args => _ => parsedArgs = args);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedArgs.Should().BeEquivalentWithSameOrdering(new[] { "a", "b", "c" });
        }

        [Fact]
        public static void GivenSomeParametersAreDefined_TheUnsedParametersShouldBeAvailableWhenCalledAsync()
        {
            var args = new[] { "a", "-x", "1", "b", "c" };
            IReadOnlyList<string>? parsedArgs = default;
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("-x").IsRequired()
                .LoadRemainingArguments()
                .Call(args => _ =>
                {
                    parsedArgs = args;
                    return Task.CompletedTask;
                });

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedArgs.Should().BeEquivalentWithSameOrdering(new[] { "a", "b", "c" });
        }

        [Fact]
        public static void GivenACustomParserIsUsedForRemainingArguments_SHouldBeHandledCorrent()
        {
            var args = new[] { "1", "2", "-1", "2" };
            IReadOnlyList<int>? parsedArgs = default;
            var builder = FluentArgsBuilder.New()
                .LoadRemainingArguments<int>()
                    .WithParser(s => int.Parse(s) * 2)
                .Call(args => parsedArgs = args);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedArgs.Should().BeEquivalentWithSameOrdering(new[] { 2, 4, -2, 4 });
        }
    }
}
