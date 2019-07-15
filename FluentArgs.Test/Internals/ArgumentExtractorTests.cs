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
        public static void ExtractingArgumentWithoutFollowingArguments_ShouldWork(string argument, string[] allArgmnets)
        {
            var extractor = new ArgumentExtractor(allArgmnets);

            var success = extractor.TryExtract(argument, out var extractedArguments, out var _);

            success.Should().BeTrue();
            extractedArguments.Should().BeEquivalentWithSameOrdering(argument);
        }

        [Theory]
        [InlineData("--key", 1, new[] { "--key", "2" }, new[] { "--key", "2" })]
        [InlineData("-a", 2, new[] { "-b", "-a", "", "-d", "-e" }, new[] { "-a", "", "-d" })]
        [InlineData("c", 1, new[] { "-c", "c", "2" }, new[] { "c", "2" })]
        public static void ExtractingArgumentWithFollowingArguments_ShouldWork(
            string argument,
            int followingArguments,
            string[] allArguments,
            string[] expectedArguments)
        {
            var extractor = new ArgumentExtractor(allArguments);

            var success = extractor.TryExtract(argument, out var extractedArguments, out var _, followingArguments);

            success.Should().BeTrue();
            extractedArguments.Should().BeEquivalentWithSameOrdering(expectedArguments); //TODO: use everywhere the method with ordering
        }

        [Fact]
        public static void ExtractingArgumentWithTooFewFollowingArguments_ShouldNotWork()
        {
            var args = new[] { "-a", "1" };
            IArgumentExtractor extractor = new ArgumentExtractor(args);

            var success = extractor.TryExtract("-a", out var extractedArguments, out var _, 2);

            success.Should().BeFalse();
        }

        [Fact]
        public static void ExtractingArgumentsWithMultipleCandidates_ShouldFail()
        {
            var args = new[] { "-a", "-a" };
            IArgumentExtractor extractor = new ArgumentExtractor(args);

            var success = extractor.TryExtract("-a", out var extractedArguments, out var _);

            success.Should().BeFalse();
            //extractAction.Should().Throw<ArgumentException>(); //TODO: ugly interface~?
        }

        [Fact]
        public static void ExtractingArgumentWithMultipleButOnlyOneValideCandidate_ShouldWork()
        {
            var args = new[] { "-c", "-k", "-a", "1", "-b", "2", "3", "-c", "x" };
            IArgumentExtractor extractor = new ArgumentExtractor(args);

            var success = extractor.TryExtract("-c", out var extractedArguments, out var _, 2);

            success.Should().BeTrue();
            extractedArguments.Should().BeEquivalentWithSameOrdering("-c", "-k", "-a");
        }

        [Fact]
        public static void ExtractingMultipleSerialArguments_ShouldWork()
        {
            var args = new[] { "-a", "1", "-b", "2", "-c", "3", "dummy", "-d", "x" };
            IArgumentExtractor extractor = new ArgumentExtractor(args);

            var success = extractor.TryExtract("-a", out var extractedArgumentsA, out extractor, 1);
            success = extractor.TryExtract("-b", out var extractedArgumentsB, out extractor, 1) && success;
            success = extractor.TryExtract("-d", out var extractedArgumentsD, out extractor, 1) && success;
            success = extractor.TryExtract("-c", out var extractedArgumentsC, out var _, 1) && success;

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

            var success1 = extractor.TryExtract("-a", out var extractedArguments1, out extractor);
            var success2 = extractor.TryExtract("-a", out var extractedArguments2, out var _);

            success1.Should().BeTrue();
            success2.Should().BeFalse();
            extractedArguments1.Should().BeEquivalentWithSameOrdering("-a");
        }

        [Fact]
        public static void ExtractingNestedArguments_ShouldNotWork()
        {
            var args = new[] { "-b", "-a", "1", "2" };
            IArgumentExtractor extractor = new ArgumentExtractor(args);

            var successA = extractor.TryExtract("-a", out var extractedArgumentsA, out extractor, 1);
            var successB = extractor.TryExtract("-b", out var extractedArgumentsB, out var _, 1);

            successA.Should().BeTrue();
            successB.Should().BeFalse();
            extractedArgumentsA.Should().BeEquivalentWithSameOrdering("-a", "1");
        }
    }
}
