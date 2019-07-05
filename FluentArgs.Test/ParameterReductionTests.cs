namespace FluentArgs.Test
{
    using System;
    using FluentAssertions;
    using Xunit;

    public static class ParameterReductionTests
    {
        [Fact]
        public static void IfAParameterIsUsedAndRemoved_TheSurroundingElementsShouldNotProduceANewParameter()
        {
            var args = new[] { "--a", "--b", "b", "a" };
            var builder = FluentArgsBuilder.New()
                .Parameter("--a").IsRequired()
                .Parameter("--b").IsRequired()
                .Call(b => a => { });

            Action parseAction = () => builder.Parse(args);

            parseAction.Should().Throw<Exception>();
        }

        [Fact]
        public static void IfAParameterIsUsedAndRemoved_TheSurroundingElementsShouldNotProduceANewParsableParameter()
        {
            var args = new[] { "--b", "--a", "a", "b" };
            string? parsedA = default;
            string? parsedB = default;
            var builder = FluentArgsBuilder.New()
                .Parameter("--a").IsOptional()
                .Parameter("--b").IsOptional()
                .Call(b => a =>
                {
                    parsedA = a;
                    parsedB = b;
                });

            builder.Parse(args);

            parsedA.Should().Be("a");
            parsedB.Should().BeNull();
        }

        //[Fact]
        //public static void IfAParameterIsUsedAndRemoved_TheSurroundingElementMightBeUsedAsFlag()
        //{
        //    var args = new[] { "--b", "--a", "a", "b" };
        //    string? parsedA = default;
        //    string? parsedB = default;
        //    var builder = FluentArgsBuilder.New()
        //        .Parameter("--a").IsOptional()
        //        .Flag("--b").IsRequired() //TODO: WTF? A flag is never required (this would not make sense)
        //        .Call(b => a =>
        //        {
        //            parsedA = a;
        //            parsedB = b;
        //        });

        //    builder.Parse(args);

        //    parsedA.Should().Be("a");
        //    parsedB.Should().BeNull();
        //}
    }
}
