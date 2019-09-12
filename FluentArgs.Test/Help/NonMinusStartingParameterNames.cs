﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using FluentAssertions;
using Xunit;

namespace FluentArgs.Test.Help
{
    public static class NonMinusStartingParameterNames
    {
        [Theory]
        [InlineData("x")]
        [InlineData("xx")]
        [InlineData("XXX")]
        [InlineData(" X ")]
        [InlineData("Today is a good day, I guess...")]
        public static void IfNotConfigured_AnyNonEmptyNameIsValid(string name)
        {
            var args = new[] { name };
            var called = false;
            var builder = FluentArgsBuilder.New()
                .Flag(name)
                .Call(_ => called = true);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            called.Should().BeTrue();
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("\t ")]
        public static void Whitespaces_ShouldAlwaysThrow(string name)
        {
            Action buildAction = () => FluentArgsBuilder.New()
                .Flag(name)
                .Call(_ => { });

            buildAction.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("-")]
        [InlineData("-x")]
        [InlineData("--x")]
        [InlineData("----jaja")]
        [InlineData("-hey du")]
        public static void MinusStartingNames_ShouldNeverThrow(string name)
        {
            var args = new[] { name };
            var called = false;
            var builder = FluentArgsBuilder.New()
                .ThrowOnNonMinusStartingNames()
                .Flag(name)
                .Call(_ => called = true);

            var parseSuccess = builder.Parse(args);

            parseSuccess.Should().BeTrue();
            called.Should().BeTrue();
        }

        [Theory]
        [InlineData("x")]
        [InlineData("xx")]
        [InlineData("XXX")]
        [InlineData(" X ")]
        [InlineData("Today is a good day, I guess...")]
        public static void IfConfiguredNonMinusStatingNames_ShouldThrow(string name)
        {
            Action buildAction = () => FluentArgsBuilder.New()
                .ThrowOnNonMinusStartingNames()
                .Flag(name)
                .Call(_ => { });

            buildAction.Should().Throw<Exception>();
        }
    }
}
