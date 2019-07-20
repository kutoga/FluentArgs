namespace FluentArgs.Test.Parsing.Given
{
    using FluentAssertions;
    using Xunit;

    public static class GivenFlagTests
    {
        [Fact]
        public static void NotGivenAFlag_ShouldNotRedirect()
        {
            bool? redirected = null;
            var args = new[] { "--anotherflag" };
            var builder = FluentArgsBuilder.New()
                .Given.Flag("--silent").Then(() => redirected = true)
                .Call(() => redirected = false);

            builder.Parse(args);

            redirected.Should().BeFalse();
        }

        [Fact]
        public static void GivenAFlag_ShouldRedirect()
        {
            bool? redirected = null;
            var args = new[] { "--silent" };
            var builder = FluentArgsBuilder.New()
                .Given.Flag("--silent").Then(() => redirected = true)
                .Call(() => redirected = false);

            builder.Parse(args);

            redirected.Should().BeTrue();
        }

        [Theory]
        [InlineData("branch1")]
        [InlineData("branch2")]
        [InlineData("branch3")]
        [InlineData("none")]
        public static void GivenAFlagAndMultipleArePossible_ShouldBeHandledCorrect(string branchName)
        {
            var args = new[] { $"--{branchName}" };
            string? calledBranch = null;
            var builder = FluentArgsBuilder.New()
                .Given.Flag("--branch1").Then(() => calledBranch = "branch1")
                .Given.Flag("--branch2").Then(() => calledBranch = "branch2")
                .Given.Flag("--branch3").Then(() => calledBranch = "branch3")
                .Call(() => calledBranch = "none");

            builder.Parse(args);

            calledBranch.Should().Be(branchName);
        }

        [Theory]
        [InlineData("branch10", "branch11")]
        [InlineData("branch10", "branch12")]
        [InlineData("branch10", "none")]
        [InlineData("none", "none")]
        public static void GivenNestedFlagRequirements_ShouldBeHandledCorrect(string branch1, string branch2)
        {
            var args = new[] { $"--{branch1}", $"--{branch2}" };
            string? calledBranch = null;
            var builder = FluentArgsBuilder.New()
                .Given.Flag("--branch10").Then(b => b
                    .Given.Flag("--branch11").Then(() => calledBranch = "branch10_branch11")
                    .Given.Flag("--branch12").Then(() => calledBranch = "branch10_branch12")
                    .Call(() => calledBranch = "branch10_none"))
                .Call(() => calledBranch = "none_none");

            builder.Parse(args);

            calledBranch.Should().Be($"{branch1}_{branch2}");
        }
    }
}
