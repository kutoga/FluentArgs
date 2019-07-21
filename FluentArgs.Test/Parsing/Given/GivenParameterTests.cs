namespace FluentArgs.Test.Parsing.Given
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using FluentArgs.Test.Helpers;
    using FluentAssertions;
    using Xunit;

    public static class GivenParameterTests
    {
        [Fact]
        public static void NotGivenAParameter_ShouldNotRedirect()
        {
            bool? redirected = null;
            var args = new[] { "--param2", "value" };
            var builder = FluentArgsBuilder.New()
                .Given.Parameter("--param")
                    .Exists()
                    .Then(() => redirected = true)
                .Call(() => redirected = false);

            builder.Parse(args);

            redirected.Should().BeFalse();
        }

        [Fact]
        public static void GivenAParameterAndAllowingAnyValue_ShouldRedirect()
        {
            bool? redirected = null;
            var args = new[] { "--param", "value" };
            var builder = FluentArgsBuilder.New()
                .Given.Parameter("--param")
                    .Exists()
                    .Then(() => redirected = true)
                .Call(() => redirected = false);

            builder.Parse(args);

            redirected.Should().BeTrue();
        }

        [Fact]
        public static void GivenAParameterWithTheCorrectValue_ShouldRedirect()
        {
            bool? redirected = null;
            var args = new[] { "--param", "value" };
            var builder = FluentArgsBuilder.New()
                .Given.Parameter("--param")
                    .HasValue("value")
                    .Then(() => redirected = true)
                .Call(() => redirected = false);

            builder.Parse(args);

            redirected.Should().BeTrue();
        }

        [Fact]
        public static void GivenAnIntParameterWithTheCorrectValue_ShouldRedirect()
        {
            bool? redirected = null;
            var args = new[] { "--param", "12" };
            var builder = FluentArgsBuilder.New()
                .Given.Parameter("--param")
                    .HasValue(12)
                    .Then(() => redirected = true)
                .Call(() => redirected = false);

            builder.Parse(args);

            redirected.Should().BeTrue();
        }

        [Fact]
        public static void GivenAMultipleParametersWithTheCorrectValue_ShouldCallTheFirst()
        {
            var calledBranches = new HashSet<string>();
            var args = new[] { "--param1", "value1", "--param2", "value2" };
            var builder = FluentArgsBuilder.New()
                .Given.Parameter("--param1")
                    .HasValue("value1")
                    .Then(() => calledBranches.Add("param1"))
                .Given.Parameter("--param2")
                    .HasValue("value2")
                    .Then(() => calledBranches.Add("param2"))
                .Call(() => calledBranches.Add("none"));

            builder.Parse(args);

            calledBranches.Should().BeEquivalentWithSameOrdering(new[] { "param1" });
        }

        [Fact]
        public static void GivenNestedParameters_ShouldBeCalled()
        {
            string? calledBranch = null;
            var args = new[] { "-p1", "v1", "-p2", "v2" };
            var builder = FluentArgsBuilder.New()
                .Given.Parameter("-p1")
                    .HasValue("v1")
                    .Then(b => b
                        .Given.Parameter("-p2")
                            .HasValue("v2")
                            .Then(() => calledBranch = "v1v2")
                        .Call(() => calledBranch = "v1"))
                .Call(() => calledBranch = "none");

            builder.Parse(args);

            calledBranch.Should().Be("v1v2");
        }

        [Fact]
        public static void GivenAParameterWithoutAValue_ShouldNotRedirect()
        {
            var args = new[] { "--param" };
            bool? redirected = null;
            var builder = FluentArgsBuilder.New()
                .Given.Parameter("--param")
                   .Exists()
                   .Then(() => redirected = true)
                .Call(() => redirected = false);

            builder.Parse(args);

            redirected.Should().BeFalse();
        }

        [Fact]
        public static void GivenAnIntParameter_ShouldBeRedirected()
        {
            bool? redirected = null;
            var args = new[] { "--age", "28" };
            var builder = FluentArgsBuilder.New()
                .Given.Parameter("--age")
                    .HasValue(28)
                    .Then(() => redirected = true)
                .Call(() => redirected = false);

            builder.Parse(args);

            redirected.Should().BeTrue();
        }

        [Fact]
        public static void GivenAnCustomParsedParameter_ShouldBeRedirected()
        {
            bool? redirected = null;
            var args = new[] { "--lowername", "beni" };
            var builder = FluentArgsBuilder.New()
                .Given.Parameter("--lowername")
                    .HasValue("BENI", s => s.ToUpper(CultureInfo.InvariantCulture))
                    .Then(() => redirected = true)
                .Call(() => redirected = false);

            builder.Parse(args);

            redirected.Should().BeTrue();
        }
    }
}
