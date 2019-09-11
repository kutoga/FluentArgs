using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace FluentArgs.Test.Help
{
    public static class NonMinusStartingParameterNames
    {
        [Fact]
        public static void IfNotConfigured_AnyNonEmptyNameIsValid()
        {
            false.Should().BeTrue();
        }

        [Fact]
        public static void Whitespaces_ShouldAlwaysThrow()
        {
            false.Should().BeTrue();
        }
    }
}
