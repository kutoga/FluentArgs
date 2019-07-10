namespace FluentArgs.Test
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;

    public static class CallWithAdditionalArgumentsTests
    {
        [Fact]
        public static void GivenNoParametersAreDefined_AllParametersShouldBeAdditionalArguments()
        {
            var args = new[] { "-x", "a", "-y", "b" };
            IReadOnlyList<string>? parsedArgs = default;
            var builder = FluentArgsBuilder.New()
                .Call(args => parsedArgs = args);

            builder.Parse(args);

            parsedArgs.Should().BeEquivalentTo(new[] { "-x", "a", "-y", "b" });
        }

        [Fact]
        public static void GivenNoParametersAreDefined_AllParametersShouldBeAdditionalArgumentsWhenCalledAsync()
        {
            var args = new[] { "-x", "a", "-y", "b" };
            IReadOnlyList<string>? parsedArgs = default;
            var builder = FluentArgsBuilder.New()
                .Call(async args => parsedArgs = args);

            builder.Parse(args);

            parsedArgs.Should().BeEquivalentTo(new[] { "-x", "a", "-y", "b" });
        }

        [Fact]
        public static void GivenSomeParametersAreDefined_TheUnsedParametersShouldBeAvailable()
        {
            var args = new[] { "a", "-x", "1", "b", "c" };
            IReadOnlyList<string>? parsedArgs = default;
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("-x").IsRequired()
                .Call(args => _ => parsedArgs = args);

            builder.Parse(args);

            parsedArgs.Should().BeEquivalentTo(new[] { "a", "b", "c" });
        }

        [Fact]
        public static void GivenSomeParametersAreDefined_TheUnsedParametersShouldBeAvailableWhenCalledAsync()
        {
            var args = new[] { "a", "-x", "1", "b", "c" };
            IReadOnlyList<string>? parsedArgs = default;
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("-x").IsRequired()
                .Call(args => _ =>
                {
                    parsedArgs = args;
                    return Task.CompletedTask;
                });

            builder.Parse(args);

            parsedArgs.Should().BeEquivalentTo(new[] { "a", "b", "c" });
        }
    }
}
