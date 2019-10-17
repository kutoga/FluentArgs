namespace FluentArgs.Test.Parsing
{
    using FluentAssertions;
    using Xunit;

    public static class EnumParserTests
    {
        public enum MyEnum
        {
            NameA,
            NameB,
            Nameb
        }

        [Theory]
        [InlineData("NameA", MyEnum.NameA)]
        [InlineData("NameB", MyEnum.NameB)]
        [InlineData("Nameb", MyEnum.Nameb)]
        public static void GivenAnEnumParameter_ParsingShouldWork(string argument, MyEnum expectedM)
        {
            var args = new[] { "-m", argument };
            MyEnum? parsedM = default;
            var builder = FluentArgsBuilder.New()
                .Parameter<MyEnum>("-m").IsRequired()
                .Call(m => parsedM = m);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedM.Should().Be(expectedM);
        }

        [Fact]
        public static void GivenAnEnumParameter_ParsingShouldWorkCaseInsensitive()
        {
            var args = new[] { "-m", "naMea" };
            MyEnum? parsedM = default;
            var builder = FluentArgsBuilder.New()
                .Parameter<MyEnum>("-m").IsRequired()
                .Call(m => parsedM = m);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedM.Should().Be(MyEnum.NameA);
        }

        [Fact]
        public static void GivenAnAmbiguousEnumParameter_ParsingShouldNotWork()
        {
            var args = new[] { "-m", "naMeB" };
            var builder = FluentArgsBuilder.New()
                .Parameter<MyEnum>("-m").IsRequired()
                .Call(m => { });

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeFalse();
        }
    }
}
