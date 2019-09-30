namespace FluentArgs.Test.Parsing
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;

    public static class SingleParameterTests
    {

        [Fact]
        public static void GivenASingleRequiredStringArgument_ShouldBeParsable()
        {
            var args = new[] { "--name", "beni" };
            string? parsedName = null;
            var builder = FluentArgsBuilder.New()
                .Parameter("--name").IsRequired()
                .Call(name => parsedName = name);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedName.Should().Be("beni");
        }

        [Fact]
        public static void GivenASingleRequiredStringArgumentWithAnAssigmentOperator_ShouldBeParsable()
        {
            var args = new[] { "--name=beni" };
            string? parsedName = null;
            var builder = FluentArgsBuilder.New()
                .Parameter("--name").IsRequired()
                .Call(name => parsedName = name);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
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

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedAge.Should().Be(28);
        }

        [Fact]
        public static void GivenARequiredArgIsMissing_ShouldNotbeParsedSuccessful()
        {
            var args = new[] { "--name", "beni" };
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("--age").IsRequired()
                .Call(age => { });

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeFalse();
        }

        [Fact]
        public static void GivenARequiredParameterWithoutAValue_ShouldNotbeParsedSuccessful()
        {
            var args = new[] { "--name" };
            var builder = FluentArgsBuilder.New()
                .Parameter("--name").IsRequired()
                .Call(name => { });

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeFalse();
        }

        [Fact]
        public static void GivenAnOptionalIntArgIsMissing_ShouldBeDefault()
        {
            var args = new[] { "--name", "beni" };
            int parsedAge = default;
            var done = false;
            var builder = FluentArgsBuilder.New()
                .Parameter<int>("--age").IsOptional()
                .Call(age =>
                {
                    parsedAge = age;
                    done = true;
                });

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            done.Should().BeTrue();
            parsedAge.Should().Be(default);
        }

        [Fact]
        public static void GivenAnOptionalStringArgIsMissing_ShouldBeDefault()
        {
            var args = new[] { "--age", "28" };
            string? parsedName = null;
            var done = false;
            var builder = FluentArgsBuilder.New()
                .Parameter("--name").IsOptional()
                .Call(name =>
                {
                    parsedName = name;
                    done = true;
                });

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            done.Should().BeTrue();
            parsedName.Should().Be(default);
        }

        [Fact]
        public static void GivenAnOptionalShortArgWithDefaultIsMissing_ShouldBeDefault()
        {
            var args = new[] { "--name", "beni" };
            short parsedAge = default;
            var done = false;
            var builder = FluentArgsBuilder.New()
                .Parameter<short>("--age").IsOptionalWithDefault(1729)
                .Call(age =>
                {
                    parsedAge = age;
                    done = true;
                });

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            done.Should().BeTrue();
            parsedAge.Should().Be(1729);
        }

        [Fact]
        public static void GivenAnOptionalShortArgWithDefaultIsAvailable_ShouldBeOverwritten()
        {
            var args = new[] { "--age", "28" };
            short parsedAge = default;
            var done = false;
            var builder = FluentArgsBuilder.New()
                .Parameter<short>("--age").IsOptionalWithDefault(1729)
                .Call(age =>
                {
                    parsedAge = age;
                    done = true;
                });

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            done.Should().BeTrue();
            parsedAge.Should().Be(28);
        }

        [Fact]
        public static void GivenAParameterWithACustomParser_ShouldBeParsed()
        {
            var args = new[] { "--lowername", "beni" };
            string? parsedName = default;
            var done = false;
            var builder = FluentArgsBuilder.New()
                .Parameter("--lowername")
                    .WithParser(s => s.ToUpper(CultureInfo.InvariantCulture))
                    .IsRequired()
                .Call(name =>
                {
                    parsedName = name;
                    done = true;
                });

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            done.Should().BeTrue();
            parsedName.Should().Be("BENI");
        }
    }
}
