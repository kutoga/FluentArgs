namespace FluentArgs.Test.Parsing
{
    using FluentAssertions;
    using System;
    using Xunit;

    public static class EnumParser
    {
        public enum MyEnum
        {
            NameA,
            NameB,
            nameb
        }

        [Theory]
        [InlineData("NameA", MyEnum.NameA)]
        [InlineData("NameB", MyEnum.NameB)]
        [InlineData("nameb", MyEnum.nameb)]
        public static void GivenAnEnumParameter_ParsingShouldWork(string argument, MyEnum expectedM)
        {
            var args = new[] { "-m", argument };
            MyEnum? parsedM = default;
            var builder = FluentArgsBuilder.New()
                .Parameter<MyEnum>("-m").IsRequired()
                .Call(m => parsedM = m);

            builder.Parse(args);

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

            builder.Parse(args);

            parsedM.Should().Be(MyEnum.NameA);
        }

        [Fact]
        public static void GivenAnAmbiguousEnumParameter_ParsingShouldNotWork()
        {
            var args = new[] { "-m", "naMeB" };
            var builder = FluentArgsBuilder.New()
                .Parameter<MyEnum>("-m").IsRequired()
                .Call(m => { });

            Action parseAction = () => builder.Parse(args);

            parseAction.Should().Throw<ArgumentException>();
        }
    }
}
