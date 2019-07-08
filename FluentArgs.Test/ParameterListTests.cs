﻿using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace FluentArgs.Test
{
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

            builder.Parse(args);

            parsedN.Should().BeEquivalentTo(new[] { 1, 2, 3 });
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

            builder.Parse(args);

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

            builder.Parse(args);

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

            builder.Parse(args);

            parsedN.Should().BeEquivalentTo(new[] { 1, 2, 4 }); //TODO: SHould().be(...)
        }

        [Fact]
        public static void GivenAParameterList_UsesDefaultSeparators()
        {
            var args = new[] { "-n", "1,2;3;1;44,1337" };
            IReadOnlyList<int> parsedN = default;
            var builder = FluentArgsBuilder.New()
                .ParameterList<int>("-n").IsRequired()
                .Call(n => parsedN = n);

            builder.Parse(args);

            parsedN.Should().BeEquivalentTo(new[] { 1, 2, 3, 1, 44, 1337 }); //TODO: SHould().be(...)
        }

        [Theory]
        [InlineData("1,2;3", ";", new[] { "1,2", "3" })]
        [InlineData("1,2;3", ",", new[] { "1", "2;3" })]
        [InlineData(",", ",", new[] { "", ""})]
        [InlineData("eigenartig", "i", new[] { "e", "genart", "g"})]
        public static void GivenAParameterListWithCustomSeparators_ShouldBeHandledCorrect(string sArg, string separator, string[] expectedValues)
        {
            var args = new[] { "-s", sArg };
            IReadOnlyList<string> parsedS = default;
            var builder = FluentArgsBuilder.New()
                .ParameterList("-s")
                    .WithSeparator(separator)
                    .IsRequired()
                .Call(s => parsedS = s);

            builder.Parse(args);

            parsedS.Should().BeEquivalentTo(expectedValues); //TODO: SHould().be(...)
        }

        [Fact]
        public static void GivenAParameterListWithACustomParser_ShouldBeHandledCorrect()
        {
            var args = new[] { "-s", "a,b" };
            IReadOnlyList<string> parsedS = default;
            var builder = FluentArgsBuilder.New()
                .ParameterList("-s")
                    .WithParser(s => s.ToUpperInvariant())
                    .IsRequired()
                .Call(s => parsedS = s);

            builder.Parse(args);

            parsedS.Should().BeEquivalentTo(new[] { "A", "B" }); //TODO: SHould().be(...)
        }
    }
}
