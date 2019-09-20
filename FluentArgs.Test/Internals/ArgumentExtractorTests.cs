﻿namespace FluentArgs.Test.Internals
{
    using System;
    using FluentArgs.ArgumentExtraction;
    using FluentArgs.Test.Helpers;
    using FluentAssertions;
    using Xunit;

    public static class ArgumentExtractorTests
    {
        [Theory]
        [InlineData("--myargument", new[] { "--myargument", "a", "-b" })]
        [InlineData("myarg", new[] { "-x", "myarg", "-b" })]
        [InlineData("-f", new[] { "-a", "--b", "-f" })]
        public static void ExtractingFlagArgument_ShouldWork(string flagName, string[] allArgmnets)
        {
            var extractor = new ArgumentExtractor(allArgmnets);

            var success = extractor.TryExtractFlag(flagName, out _);

            success.Should().BeTrue();
        }

        [Theory]
        [InlineData("--key", 1, new[] { "--key", "2" }, "2")]
        [InlineData("c", 1, new[] { "-c", "c", "2" }, "2")]
        public static void ExtractingNamedArgumentWithFollowingArguments_ShouldWork(
            string argument,
            int followingArguments,
            string[] allArguments,
            string expectedValue)
        {
            var extractor = new ArgumentExtractor(allArguments);

            var success = extractor.TryExtractNamedArgument(argument, out var value, out _);

            success.Should().BeTrue();
            value.Should().Be(expectedValue);
        }

        [Fact]
        public static void ExtractingNamedArgumentsWithoutAValue_ShouldNotWork()
        {
            var args = new[] { "-a" };
            IArgumentExtractor extractor = new ArgumentExtractor(args);

            var success = extractor.TryExtractNamedArgument("-a", out var value, out _);

            success.Should().BeFalse();
        }

        [Fact]
        public static void ExtractingArgumentsWithMultipleCandidates_ShouldFail()
        {
            var args = new[] { "-a", "-a" };
            IArgumentExtractor extractor = new ArgumentExtractor(args);

            var success = extractor.TryExtractNamedArgument("-a", out var extractedArguments, out var _);

            success.Should().BeFalse();
            //extractAction.Should().Throw<ArgumentException>(); //TODO: ugly interface~?
        }

        [Fact]
        public static void ExtractingArgumentWithMultipleButOnlyOneValideCandidate_ShouldWork()
        {
            var args = new[] { "-c", "-k", "-a", "1", "-b", "2", "3", "-c", "x" };
            IArgumentExtractor extractor = new ArgumentExtractor(args);

            var success = extractor.TryExtractNamedArgument("-c", out var extractedArguments, out var _, 2);

            success.Should().BeTrue();
            extractedArguments.Should().BeEquivalentWithSameOrdering("-c", "-k", "-a");
        }

        [Fact]
        public static void ExtractingMultipleSerialArguments_ShouldWork()
        {
            var args = new[] { "-a", "1", "-b", "2", "-c", "3", "dummy", "-d", "x" };
            IArgumentExtractor extractor = new ArgumentExtractor(args);

            var success = extractor.TryExtractNamedArgument("-a", out var extractedArgumentsA, out extractor);
            success = extractor.TryExtractNamedArgument("-b", out var extractedArgumentsB, out extractor) && success;
            success = extractor.TryExtractNamedArgument("-d", out var extractedArgumentsD, out extractor) && success;
            success = extractor.TryExtractNamedArgument("-c", out var extractedArgumentsC, out var _) && success;

            success.Should().BeTrue();
            extractedArgumentsA.Should().BeEquivalentWithSameOrdering("-a", "1");
            extractedArgumentsB.Should().BeEquivalentWithSameOrdering("-b", "2");
            extractedArgumentsC.Should().BeEquivalentWithSameOrdering("-c", "3");
            extractedArgumentsD.Should().BeEquivalentWithSameOrdering("-d", "x");
        }

        [Fact]
        public static void ExtractTheSameArgumentTwice_ShouldNotWork()
        {
            var args = new[] { "-a" };
            IArgumentExtractor extractor = new ArgumentExtractor(args);

            var success1 = extractor.TryExtractFlag("-a", out _);
            var success2 = extractor.TryExtractFlag("-a", out _);

            success1.Should().BeTrue();
            success2.Should().BeFalse();
        }

        [Fact]
        public static void ExtractingNestedNamedArguments_ShouldNotWork()
        {
            var args = new[] { "-b", "-a", "1", "2" };
            IArgumentExtractor extractor = new ArgumentExtractor(args);

            var successA = extractor.TryExtractNamedArgument("-a", out var extractedValueA, out extractor);
            var successB = extractor.TryExtractNamedArgument("-b", out var extractedValueB, out var _);

            successA.Should().BeTrue();
            successB.Should().BeFalse();
            extractedValueA.Should().Be("1");
        }

        [Fact]
        public static void ExtractingArgumentsWithEqualSign_ShouldWork()
        {
            var args = new[] { "-a=1" };
            IArgumentExtractor extractor = new ArgumentExtractor(args);

            var success = extractor.TryExtractNamedArgument(new[] { "-a" }, out var extractedArguments, out var _, 1);

            success.Should().BeTrue();
            extractedArguments.Should().BeEquivalentWithSameOrdering("-a", "1");
        }

        [Fact]
        public static void PositionalArgument_ShouldReturnAndRemoveTheFirstArgument()
        {
            var args = new[] {"a", "b"};
            IArgumentExtractor extractor = new ArgumentExtractor(args);

            var successA = extractor.TryPopArgument(out var extractedArgumentA, out extractor);
            var successB = extractor.TryPopArgument(out var extractedArgumentB, out var _);

            successA.Should().BeTrue();
            successB.Should().BeTrue();
            extractedArgumentA.Should().Be("a");
            extractedArgumentB.Should().Be("b");
        }

        [Fact]
        public static void PositionalArgumentWhenThereAreNoArguments_ShouldFail()
        {
            var args = Array.Empty<string>();
            IArgumentExtractor extractor = new ArgumentExtractor(args);

            var success = extractor.TryPopArgument(out var extractedArgument, out var _);

            success.Should().BeFalse();
        }
    }
}
