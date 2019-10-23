namespace FluentArgs.Test.Parsing
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;

    public static class DefaultParserTests
    {
        public enum TestEnum
        {
            Value1,
            Value2
        }

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
        [InlineData("0x32", (int)0x32)]
        public static void DefaultIntegerParser_ShouldExist(string value, int expectedParsedValue)
        {
            ParseValueWithDefaultParser<int>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("-9", -9)]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        [InlineData("0x32", (int)0x32)]
        public static void DefaultNullableIntegerParser_ShouldExist(string value, int expectedParsedValue)
        {
            ParseValueWithDefaultParser<int?>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        [InlineData("0x32", 0x32u)]
        public static void DefaultUnsignedIntegerParser_ShouldExist(string value, uint expectedParsedValue)
        {
            ParseValueWithDefaultParser<uint>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        [InlineData("0x32", 0x32u)]
        public static void DefaultNullableUnsignedIntegerParser_ShouldExist(string value, uint expectedParsedValue)
        {
            ParseValueWithDefaultParser<uint?>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("-9", -9)]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        [InlineData("0x32", (short)0x32)]
        public static void DefaultShortParser_ShouldExist(string value, short expectedParsedValue)
        {
            ParseValueWithDefaultParser<short>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("-9", -9)]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        [InlineData("0x32", (short)0x32)]
        public static void DefaultNullableShortParser_ShouldExist(string value, short expectedParsedValue)
        {
            ParseValueWithDefaultParser<short?>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        [InlineData("0x32", (ushort)0x32)]
        public static void DefaultUnsignedShortParser_ShouldExist(string value, ushort expectedParsedValue)
        {
            ParseValueWithDefaultParser<ushort>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        [InlineData("0x32", (ushort)0x32)]
        public static void DefaultNullableUnsignedShortParser_ShouldExist(string value, ushort expectedParsedValue)
        {
            ParseValueWithDefaultParser<ushort?>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("-9", -9)]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        [InlineData("0x32", 0x32L)]
        public static void DefaultLongParser_ShouldExist(string value, long expectedParsedValue)
        {
            ParseValueWithDefaultParser<long>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("-9", -9)]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        [InlineData("0x32", 0x32L)]
        public static void DefaultNullableLongParser_ShouldExist(string value, long expectedParsedValue)
        {
            ParseValueWithDefaultParser<long?>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        [InlineData("0x32", 0x32L)]
        public static void DefaultUnsignedLongParser_ShouldExist(string value, ulong expectedParsedValue)
        {
            ParseValueWithDefaultParser<ulong>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("0", 0)]
        [InlineData("333", 333)]
        [InlineData("0x32", 0x32L)]
        public static void DefaultNullableUnsignedLongParser_ShouldExist(string value, ulong expectedParsedValue)
        {
            ParseValueWithDefaultParser<ulong?>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("0", (byte)0)]
        [InlineData("1", (byte)1)]
        [InlineData("0x32", (byte)0x32)]
        public static void DefaultByteParser_ShouldExist(string value, byte expectedParsedValue)
        {
            ParseValueWithDefaultParser<byte>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("0", (byte)0)]
        [InlineData("1", (byte)1)]
        [InlineData("0x32", (byte)0x32)]
        public static void DefaultNullableByteParser_ShouldExist(string value, byte expectedParsedValue)
        {
            ParseValueWithDefaultParser<byte?>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("a", 'a')]
        [InlineData("0x32", (char)0x32)]
        public static void DefaultCharParser_ShouldExist(string value, char expectedParsedValue)
        {
            ParseValueWithDefaultParser<char>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("a", 'a')]
        [InlineData("0x32", (char)0x32)]
        public static void DefaultNullableCharParser_ShouldExist(string value, char expectedParsedValue)
        {
            ParseValueWithDefaultParser<char?>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData("")]
        [InlineData("ab")]
        public static void InvalidCharSequence_Shouldthrow(string value)
        {
            Action parseAction = () => ParseValueWithDefaultParser<char>(value);

            parseAction.Should().Throw<Exception>();
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

        [Theory]
        [InlineData("http://www.google.de")]
        [InlineData("ftp://wiki.com")]
        public static void DefaultUriParser_ShouldExist(string value)
        {
            ParseValueWithDefaultParser<Uri>(value)
                .Should().Be(new Uri(value));
        }

        [Theory]
        [InlineData("http://www.google.de")]
        [InlineData("ftp://wiki.com")]
        public static void DefaultNullableUriParser_ShouldExist(string value)
        {
            ParseValueWithDefaultParser<Uri?>(value)
                .Should().Be(new Uri(value));
        }

        [Theory]
        [InlineData("file.txt")]
        [InlineData("directory/file.txt")]
        public static void DefaultFileInfoParser_ShouldExist(string value)
        {
            ParseValueWithDefaultParser<FileInfo>(value)
                .Should().NotBeNull();
        }

        [Theory]
        [InlineData("file.txt")]
        [InlineData("directory/file.txt")]
        public static void DefaultNullableFileInfoParser_ShouldExist(string value)
        {
            ParseValueWithDefaultParser<FileInfo?>(value)
                .Should().NotBeNull();
        }

        [Theory]
        [InlineData("./")]
        [InlineData("directoryA/directoryB")]
        public static void DefaultDirectoryInfoParser_ShouldExist(string value)
        {
            ParseValueWithDefaultParser<DirectoryInfo>(value)
                .Should().NotBeNull();
        }

        [Theory]
        [InlineData("./")]
        [InlineData("directoryA/directoryB")]
        public static void DefaultNullableDirectoryInfoParser_ShouldExist(string value)
        {
            ParseValueWithDefaultParser<DirectoryInfo?>(value)
                .Should().NotBeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("1")]
        public static void UriParseGivenInvalidInput_ShouldThrow(string value)
        {
            Action parseAction = () => ParseValueWithDefaultParser<Uri>(value);

            parseAction.Should().Throw<Exception>();
        }

        [Theory]
        [MemberData(nameof(GetDateTimeParseValues))]
        public static void DefaultDateTimeParser_ShouldExist(string value, DateTime expectedParsedValue)
        {
            ParseValueWithDefaultParser<DateTime>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [MemberData(nameof(GetDateTimeParseValues))]
        public static void DefaultNullableDateTimeParser_ShouldExist(string value, DateTime expectedParsedValue)
        {
            ParseValueWithDefaultParser<DateTime?>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [MemberData(nameof(GetDateTimeOffsetParseValues))]
        public static void DefaultDateTimeOffsetParser_ShouldExist(string value, DateTimeOffset expectedParsedValue)
        {
            ParseValueWithDefaultParser<DateTimeOffset>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [MemberData(nameof(GetDateTimeOffsetParseValues))]
        public static void DefaultNullableDateTimeOffsetParser_ShouldExist(string value, DateTimeOffset expectedParsedValue)
        {
            ParseValueWithDefaultParser<DateTimeOffset?>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [MemberData(nameof(GetTimeSpanParseValues))]
        public static void DefaultTimeSpanParser_ShouldExist(string value, TimeSpan expectedParsedValue)
        {
            ParseValueWithDefaultParser<TimeSpan>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [MemberData(nameof(GetTimeSpanParseValues))]
        public static void DefaultNullableTimeSpanOffsetParser_ShouldExist(string value, TimeSpan expectedParsedValue)
        {
            ParseValueWithDefaultParser<TimeSpan?>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData(nameof(TestEnum.Value1), TestEnum.Value1)]
        [InlineData(nameof(TestEnum.Value2), TestEnum.Value2)]
        public static void DefaultEnumParser_ShouldExist(string value, TestEnum expectedParsedValue)
        {
            ParseValueWithDefaultParser<TestEnum>(value)
                .Should().Be(expectedParsedValue);
        }

        [Theory]
        [InlineData(nameof(TestEnum.Value1), TestEnum.Value1)]
        [InlineData(nameof(TestEnum.Value2), TestEnum.Value2)]
        public static void DefaultNullableEnumParser_ShouldExist(string value, TestEnum expectedParsedValue)
        {
            ParseValueWithDefaultParser<TestEnum?>(value)
                .Should().Be(expectedParsedValue);
        }

        private static IEnumerable<object[]> GetDateTimeParseValues()
        {
            return new[]
            {
                new object[] { "10.10.2010 10:10:10", new DateTime(2010, 10, 10, 10, 10, 10) }
            };
        }

        private static IEnumerable<object[]> GetDateTimeOffsetParseValues()
        {
            return new[]
            {
                new object[] { "10.10.2010 10:10:10 +00:00", new DateTimeOffset(2010, 10, 10, 10, 10, 10, TimeSpan.Zero),  }
            };
        }

        private static IEnumerable<object[]> GetTimeSpanParseValues()
        {
            return new[]
            {
                new object[] { "10:32:05", new TimeSpan(10, 32, 5) }
            };
        }

        private static T ParseValueWithDefaultParser<T>(string value)
        {
            var args = new[] { "-v", value };
            var resultTask = new TaskCompletionSource<T>();
            var builder = FluentArgsBuilder.New()
                .Parameter<T>("-v").IsRequired()
                .Call(v => resultTask.SetResult(v));

            var parseSuccess = builder.Parse(args);
            if (!(parseSuccess && resultTask.Task.IsCompletedSuccessfully))
            {
                throw new Exception($"Could not parse value '{value}'!");
            }

            return resultTask.Task.Result;
        }
    }
}
