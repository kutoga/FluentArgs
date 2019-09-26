namespace FluentArgs.Test.Internals
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
            var args = new[] { "-a", "1", "-a", "2" };
            IArgumentExtractor extractor = new ArgumentExtractor(args);

            var success = extractor.TryExtractNamedArgument("-a", out var extractedArguments, out _);

            success.Should().BeFalse();
            //extractAction.Should().Throw<ArgumentException>(); //TODO: ugly interface~?
        }

        [Fact]
        public static void ExtractingArgumentsWithMultipleOverlappingCandidates_ShouldFail()
        {
            var args = new[] { "-a", "-a", "2" };
            IArgumentExtractor extractor = new ArgumentExtractor(args);

            var success = extractor.TryExtractNamedArgument("-a", out var extractedArguments, out _);

            success.Should().BeFalse();
            //extractAction.Should().Throw<ArgumentException>(); //TODO: ugly interface~?
        }

        [Fact]
        public static void ExtractingNamedArgumentWithMultipleButOnlyOneValidCandidate_ShouldWork()
        {
            var args = new[] { "-c", "-k", "-a", "1", "-b", "2", "3", "-c" };
            IArgumentExtractor extractor = new ArgumentExtractor(args);

            var success = extractor.TryExtractNamedArgument("-c", out var value, out _);

            success.Should().BeTrue();
            value.Should().Be("-k");
        }

        [Fact]
        public static void ExtractingMultipleSerialNamedArguments_ShouldWork()
        {
            var args = new[] { "-a", "1", "-b", "2", "-c", "3", "dummy", "-d", "4" };
            IArgumentExtractor extractor = new ArgumentExtractor(args);

            var success = extractor.TryExtractNamedArgument("-a", out var valueA, out extractor);
            success = extractor.TryExtractNamedArgument("-b", out var valueB, out extractor) && success;
            success = extractor.TryExtractNamedArgument("-d", out var valueD, out extractor) && success;
            success = extractor.TryExtractNamedArgument("-c", out var valueC, out _) && success;

            success.Should().BeTrue();
            valueA.Should().Be("1");
            valueB.Should().Be("2");
            valueC.Should().Be("3");
            valueD.Should().Be("4");
        }

        [Fact]
        public static void ExtractTheSameArgumentTwice_ShouldNotWork()
        {
            var args = new[] { "-a" };
            IArgumentExtractor extractor = new ArgumentExtractor(args);

            var success1 = extractor.TryExtractFlag("-a", out extractor);
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
            var successB = extractor.TryExtractNamedArgument("-b", out var extractedValueB, out _);

            successA.Should().BeTrue();
            successB.Should().BeFalse();
            extractedValueA.Should().Be("1");
        }

        [Fact]
        public static void ExtractingArgumentsWithEqualSign_ShouldWork()
        {
            var args = new[] { "-a=1" };
            var extractor = new ArgumentExtractor(args);

            var success = extractor.TryExtractNamedArgument("-a", out var value, out _, new[] { "=" });

            success.Should().BeTrue();
            value.Should().Be("1");
        }

        [Fact]
        public static void ExtractingArgumentsWithDifferentAssignmentsOperators_ShouldWork()
        {
            var args = new[] { "-a=1", "-b:2" };
            var assignmentOperators = new[] { "=", ":" };
            IArgumentExtractor extractor = new ArgumentExtractor(args);

            var successA = extractor.TryExtractNamedArgument("-a", out var argA, out extractor, assignmentOperators);
            var successB = extractor.TryExtractNamedArgument("-b", out var argB, out _, assignmentOperators);

            successA.Should().BeTrue();
            successB.Should().BeTrue();
            argA.Should().Be("1");
            argB.Should().Be("2");
        }

        [Fact]
        public static void DespiteDefiningAssignemtnOperatorsExtractionWithout_ShouldWork()
        {
            var args = new[] { "-a", "1" };
            var extractor = new ArgumentExtractor(args);

            var success = extractor.TryExtractNamedArgument("-a", out var argA, out _, new[] { "=" });

            success.Should().BeTrue();
            argA.Should().Be("1");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("\t")]
        [InlineData(" \t ")]
        public static void WhitespaceAssignmentOperators_ShouldBeValid(string assignmentOperator)
        {
            var args = new[] { $"-a{assignmentOperator}xyz" };
            var extractor = new ArgumentExtractor(args);

            var success = extractor.TryExtractNamedArgument("-a", out var argA, out _, new[] { assignmentOperator });

            success.Should().BeTrue();
            argA.Should().Be("xyz");
        }

        [Fact]
        public static void PositionalArgument_ShouldReturnAndRemoveTheFirstArgument()
        {
            var args = new[] {"a", "b"};
            IArgumentExtractor extractor = new ArgumentExtractor(args);

            var successA = extractor.TryPopArgument(out var extractedArgumentA, out extractor);
            var successB = extractor.TryPopArgument(out var extractedArgumentB, out _);

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

            var success = extractor.TryPopArgument(out var extractedArgument, out _);

            success.Should().BeFalse();
        }
    }
}
