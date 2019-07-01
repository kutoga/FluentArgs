namespace FluentArgs.Test.Given
{
    using System;
    using FluentAssertions;
    using Xunit;

    public static class GivenCommandTests
    {
        [Theory]
        [InlineData("-c")]
        [InlineData("-d")]
        public static void GivenAnAlwaysIgnoredCommand_ShouldNeverBeRedirected(string arg)
        {
            var args = new[] { arg, "xxx" };
            bool? redirected = null;
            var builder = FluentArgsBuilder.New()
                .Given.Command("-c")
                    .Matches(_ => false).Then(c => c.Invalid())
                    .ElseIgnore()
                .Call(() => redirected = false);

            builder.Parse(args);

            redirected.Should().BeFalse();
        }

        [Fact]
        public static void GivenAnAlwaysInvalidCommandValue_ShouldThrow()
        {
            var args = new[] { "-c", "beni" };
            var builder = FluentArgsBuilder.New()
                .Given.Command("-c")
                    .Matches(_ => false).Then(() => { })
                    .ElseIsInvalid()
                .Call(() => { });

            Action parseAction = () => builder.Parse(args);

            parseAction.Should().Throw<Exception>();
        }

        [Fact]
        public static void GivenACommandWithAPredefinedValue_ShouldBeRedirected()
        {
            var args = new[] { "-c", "copy" };
            string? calledBranch = default; //TODO: ist überall calledBranch = default oder ab und zu = null?
            var builder = FluentArgsBuilder.New()
                .Given.Command("--command", "-c") //TODO: Extension-Methode für Then, die Then(b => b.Call(...)) aufruft
                    .HasValue("copy").Then(b => b.Call(() => calledBranch = "branch1"))
                    .ElseIgnore()
                .Call(() => calledBranch = "none");

            builder.Parse(args);

            calledBranch.Should().Be("branch1");
        }

        [Theory]
        [InlineData("branch1")]
        [InlineData("branch2")]
        [InlineData("branch3")]
        [InlineData("none")]
        public static void GivenACommandWithMultipleBranches_ShouldBeRedirected(string branch)
        {
            var args = new[] { "--command", branch };
            string? calledBranch = default;
            var builder = FluentArgsBuilder.New()
                .Given.Command("--command")
                    .HasValue("branch1").Then(() => calledBranch = "branch1")
                    .HasValue("branch2").Then(() => calledBranch = "branch2")
                    .HasValue("branch3").Then(() => calledBranch = "branch3")
                    .ElseIgnore()
                .Call(() => calledBranch = "none");

            builder.Parse(args);

            calledBranch.Should().Be(branch);
        }

        [Fact]
        public static void GivenACommandWithAnInvalidValue_ShouldThrow()
        {
            var args = new[] { "--type", "pi" };
            var builder = FluentArgsBuilder.New()
                .Given.Command("--type")
                    .HasValue("a").Then(() => { })
                    .ElseIgnore()
                .Call(() => { });

            Action parseAction = () => builder.Parse(args);

            parseAction.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("1", "branch1")]
        [InlineData("2.0", "branch2")]
        public static void GivenAMixedTypeCommandValue_ShouldBeRedirected(string value, string branch)
        {
            var args = new[] { "-x", value };
            string? calledBranch = default;
            var builder = FluentArgsBuilder.New()
                .Given.Command("-x")
                    .HasValue(1).Then(() => calledBranch = "branch1")
                    .HasValue(2.0).Then(() => calledBranch = "branch2")
                    .ElseIsInvalid()
                .Call(() => calledBranch = "none");

            true.Should().BeFalse();
        }
    }
}
