﻿namespace FluentArgs.Test.Parsing
{
    using System;
    using System.Collections.Generic;
    using FluentArgs.Test.Helpers;
    using FluentAssertions;
    using Xunit;

    public class ParameterListTests
    {
        //TODO: pack all parameter configs into a single interface
        //TODO: copy this interface and add:
        //      WithSeparator(...)
        //      IsOptionalWithEmptyDefault()
        [Fact]
        public static void GivenAParameterList_ShouldBeParsed()
        {
            var args = new[] { "-n", "1,2,3" };
            IReadOnlyList<int> parsedN = default;
            var builder = FluentArgsBuilder.New()
                .ParameterList<int>("-n").IsRequired()
                .Call(n => parsedN = n);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedN.Should().BeEquivalentWithSameOrdering(new[] { 1, 2, 3 });
        }

        [Fact]
        public static void GivenARequiredParameterListWhichIsNotPresent_ShouldThrow()
        {
            var args = new[] { "-x" };
            IReadOnlyList<int> parsedN = default;
            var builder = FluentArgsBuilder.New()
                .ParameterList<int>("-n").IsRequired()
                .Call(n => parsedN = n);

            Action parseAction = () => builder.Parse(args);

            parseAction.Should().Throw<Exception>();
        }

        [Fact]
        public static void GivenAnOptionalParameterListWhichIsNotPresent_ShouldReturnNull()
        {
            var args = new[] { "-x" };
            IReadOnlyList<int> parsedN = new int[1];
            var builder = FluentArgsBuilder.New()
                .ParameterList<int>("-n").IsOptional()
                .Call(n => parsedN = n);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedN.Should().BeNull();
        }

        [Fact]
        public static void GivenAnOptionaldParameterListWithAnEmptyDefaultWhichIsNotPresent_ShouldReturnAnEmptyArray()
        {
            var args = new[] { "-x" };
            IReadOnlyList<int> parsedN = default;
            var builder = FluentArgsBuilder.New()
                .ParameterList<int>("-n").IsOptionalWithEmptyDefault()
                .Call(n => parsedN = n);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedN.Should().BeEmpty();
        }

        [Fact]
        public static void GivenAnOptionaldParameterListWithADefaultWhichIsNotPresent_ShouldReturnDefault()
        {
            var args = new[] { "-x" };
            IReadOnlyCollection<int> parsedN = default;
            var builder = FluentArgsBuilder.New()
                .ParameterList<int>("-n").IsOptionalWithDefault(new[] { 1, 2, 4 })
                .Call(n => parsedN = n);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedN.Should().BeEquivalentWithSameOrdering(new[] { 1, 2, 4 }); //TODO: SHould().be(...)
        }

        [Fact]
        public static void GivenAParameterList_UsesDefaultSeparators()
        {
            var args = new[] { "-n", "1,2;3;1;44,1337" };
            IReadOnlyList<int> parsedN = default;
            var builder = FluentArgsBuilder.New()
                .ParameterList<int>("-n").IsRequired()
                .Call(n => parsedN = n);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedN.Should().BeEquivalentWithSameOrdering(new[] { 1, 2, 3, 1, 44, 1337 }); //TODO: SHould().be(...)
        }

        [Theory]
        [InlineData("1,2;3", ";", new[] { "1,2", "3" })]
        [InlineData("1,2;3", ",", new[] { "1", "2;3" })]
        [InlineData(",", ",", new[] { "", "" })]
        [InlineData("eigenartig", "i", new[] { "e", "genart", "g" })]
        public static void GivenAParameterListWithCustomSeparators_ShouldBeHandledCorrect(string sArg, string separator, string[] expectedValues)
        {
            var args = new[] { "-s", sArg };
            IReadOnlyList<string> parsedS = default;
            var builder = FluentArgsBuilder.New()
                .ParameterList("-s")
                    .WithSeparator(separator)
                    .IsRequired()
                .Call(s => parsedS = s);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedS.Should().BeEquivalentWithSameOrdering(expectedValues); //TODO: SHould().be(...)
        }

        [Fact]
        public static void GivenAParameterListWithACustomParser_ShouldBeHandledCorrect()
        {
            var args = new[] { "-s", "a,b" };
            IReadOnlyList<string>? parsedS = default;
            var builder = FluentArgsBuilder.New()
                .ParameterList("-s")
                    .WithParser(s => s.ToUpperInvariant())
                    .IsRequired()
                .Call(s => parsedS = s);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedS.Should().BeEquivalentWithSameOrdering(new[] { "A", "B" }); //TODO: SHould().be(...)
        }

        [Fact]
        public static void GivenMultipleParameterLists_ShouldBeHandledCorrect()
        {
            var args = new[] { "-a", "1,2,3", "-b", "3,4,5" };
            IReadOnlyList<int>? parsedA = default;
            IReadOnlyList<int>? parsedB = default;
            var builder = FluentArgsBuilder.New()
                .ParameterList<int>("-a").IsRequired()
                .ParameterList<int>("-b").IsRequired()
                .Call(b => a =>
                {
                    parsedA = a;
                    parsedB = b;
                });

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            parsedA.Should().BeEquivalentWithSameOrdering(1, 2, 3);
            parsedB.Should().BeEquivalentWithSameOrdering(3, 4, 5);
        }
    }
}
