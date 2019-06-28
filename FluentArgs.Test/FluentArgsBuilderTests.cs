namespace FluentArgs.Test
{
    using System;
    using FluentAssertions;
    using Xunit;

    public static class FluentArgsBuilderTests
    {
        [Fact]
        public static void GivenNoArgumentsAndNoParameters_ShouldBeParsable()
        {
            bool done = false;
            var args = Array.Empty<string>();
            var builder = FluentArgsBuilder.New()
                .Call(() => done = true);

            builder.Parse(args);

            done.Should().BeTrue();
        }

        [Fact]
        public static void GivenASingleRequiredStringArgument_ShouldBeParsable()
        {
            var args = new[] { "--name", "beni" };
            string? parsedName = null;
            var builder = FluentArgsBuilder.New()
                .Parameter<string>("--name").IsRequired()
                .Call(name => parsedName = name);

            builder.Parse(args);

            parsedName.Should().Be("beni");
        }

        [Fact]
        public static void GivenASingleRequiredIntArgument_ShouldBeParsable()
        {
            var args = new[] { "--age", "28" };
            int? parsedAge = null;
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("--age").IsRequired()
                .Call(age => parsedAge = age);

            builder.Parse(args);

            parsedAge.Should().Be(28);
        }

        [Fact]
        public static void GivenARequiredArgIsMissing_ShouldThrow()
        {
            var args = new[] { "--name", "beni" };
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("--age").IsRequired()
                .Call(age => { });

            Action parse = () => builder.Parse(args);

            parse.Should().Throw<Exception>();
        }

        [Fact]
        public static void GivenAnOptionalIntArgIsMissing_ShouldBeDefault()
        {
            var args = new[] { "--name", "beni" };
            int parsedAge = default;
            bool done = false;
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("--age").IsOptional()
                .Call(age =>
                {
                    parsedAge = age;
                    done = true;
                });

            builder.Parse(args);

            done.Should().BeTrue();
            parsedAge.Should().Be(default);
        }

        [Fact]
        public static void GivenAnOptionalStringArgIsMissing_ShouldBeDefault()
        {
            var args = new[] { "--age", "28" };
            string? parsedName = null;
            bool done = false;
            var builder = FluentArgsBuilder.New()
                .Parameter<string>("--name").IsOptional()
                .Call(name =>
                {
                    parsedName = name;
                    done = true;
                });

            builder.Parse(args);

            done.Should().BeTrue();
            parsedName.Should().Be(default);
        }
    }
}
