using System;
using System.Configuration;
using FluentAssertions;
using Xunit;

namespace FluentArgs.Test.Parsing
{
    public static class DefaultParserTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("xyz")]
        [InlineData("good\nmorning")]
        public static void DefaultStringParser_ShouldExist(string value)
        {
            ParseValueWithDefaultParser<string>(value)
                .Should().Be(value);
        }

        [Theory]
        [InlineData("0", 0.0d)]
        [InlineData("-1.5", -1.5d)]
        [InlineData("16.66666", 16.6666d)]
        public static void DefaultDoubleParser_ShouldExist(string value, double expectedParsedValue)
        {
            ParseValueWithDefaultParser<double>(value)
                .Should().BeApproximately(expectedParsedValue, 1e-4);
        }

        [Theory]
        [InlineData("0", 0.0d)]
        [InlineData("-1.5", -1.5d)]
        [InlineData("16.66666", 16.6666d)]
        public static void DefaultNullableDoubleParser_ShouldExist(string value, double expectedParsedValue)
        {
            ParseValueWithDefaultParser<double?>(value)
                .Should().BeApproximately(expectedParsedValue, 1e-4);
        }

        [Theory]
        [InlineData("0", 0.0f)]
        [InlineData("-1.5", -1.5f)]
        [InlineData("16.66666", 16.6666f)]
        public static void DefaultFloatParser_ShouldExist(string value, float expectedParsedValue)
        {
            ParseValueWithDefaultParser<float>(value)
                .Should().BeApproximately(expectedParsedValue, 1e-4f);
        }

        [Theory]
        [InlineData("0", 0.0f)]
        [InlineData("-1.5", -1.5f)]
        [InlineData("16.66666", 16.6666f)]
        public static void DefaultNullableFloatParser_ShouldExist(string value, float expectedParsedValue)
        {
            ParseValueWithDefaultParser<float?>(value)
                .Should().BeApproximately(expectedParsedValue, 1e-4f);
        }

        [Theory]
        [InlineData("0", 0.0d)]
        [InlineData("-1.5", -1.5d)]
        [InlineData("16.66666", 16.6666d)]
        public static void DefaultDecimalParser_ShouldExist(string value, decimal expectedParsedValue)
        {
            ParseValueWithDefaultParser<decimal>(value)
                .Should().BeApproximately(expectedParsedValue, 1e-4m);
        }

        [Theory]
        [InlineData("0", 0.0d)]
        [InlineData("-1.5", -1.5d)]
        [InlineData("16.66666", 16.6666d)]
        public static void DefaultNullableDecimalParser_ShouldExist(string value, decimal expectedParsedValue)
        {
            ParseValueWithDefaultParser<decimal?>(value)
                .Should().BeApproximately(expectedParsedValue, 1e-4m);
        }

        [Theory]
        [InlineData("-9", -9)]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        public static void DefaultIntegerParser_ShouldExist(string value, int expectedParsedValue)
        {
            ParseValueWithDefaultParser<int>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("-9", -9)]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        public static void DefaultNullableIntegerParser_ShouldExist(string value, int expectedParsedValue)
        {
            ParseValueWithDefaultParser<int?>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        public static void DefaultUnsignedIntegerParser_ShouldExist(string value, uint expectedParsedValue)
        {
            ParseValueWithDefaultParser<uint>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        public static void DefaultNullableUnsignedIntegerParser_ShouldExist(string value, uint expectedParsedValue)
        {
            ParseValueWithDefaultParser<uint?>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("-9", -9)]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        public static void DefaultShortParser_ShouldExist(string value, short expectedParsedValue)
        {
            ParseValueWithDefaultParser<short>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("-9", -9)]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        public static void DefaultNullableShortParser_ShouldExist(string value, short expectedParsedValue)
        {
            ParseValueWithDefaultParser<short?>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        public static void DefaultUnsignedShortParser_ShouldExist(string value, ushort expectedParsedValue)
        {
            ParseValueWithDefaultParser<ushort>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        public static void DefaultNullableUnsignedShortParser_ShouldExist(string value, ushort expectedParsedValue)
        {
            ParseValueWithDefaultParser<ushort?>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("-9", -9)]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        public static void DefaultLongParser_ShouldExist(string value, long expectedParsedValue)
        {
            ParseValueWithDefaultParser<long>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("-9", -9)]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        public static void DefaultNullableLongParser_ShouldExist(string value, long expectedParsedValue)
        {
            ParseValueWithDefaultParser<long?>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        public static void DefaultUnsignedLongParser_ShouldExist(string value, ulong expectedParsedValue)
        {
            ParseValueWithDefaultParser<ulong>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        public static void DefaultNullableUnsignedLongParser_ShouldExist(string value, ulong expectedParsedValue)
        {
            ParseValueWithDefaultParser<ulong?>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        public static void DefaultBooleanParser_ShouldExist(string value, bool expectedParsedValue)
        {
            ParseValueWithDefaultParser<bool>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        public static void DefaultNullableBooleanParser_ShouldExist(string value, bool expectedParsedValue)
        {
            ParseValueWithDefaultParser<bool?>(value)
                .Should().Be(expectedParsedValue);
        }

        [Fact]
        public static void Reminder()
        {
            //TODO:
            //datetime
            //datetimeoffset
            //timespan
            //enum

            //TODO:
            //byte
            //0xabc parsing (for numbers?)

            true.Should().Be(false);
        }

        private static T ParseValueWithDefaultParser<T>(string value)
        {
            var args = new[] {"-v", value};
            var result = default(T);
            var called = false;
            var builder = FluentArgsBuilder.New()
                .Parameter<T>("-v").IsRequired()
                .Call(v =>
                {
                    result = v;
                    called = true;
                });

            var parseSuccess = builder.Parse(args);
            if (!(parseSuccess && called))
            {
                throw new Exception($"Could not parse value '{value}'!");
            }

            return result;
        }
    }
}
