namespace FluentArgs.Test.Parsing.Given
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
                    .ElseIsInvalid()
                .Call(() => { });

            Action parseAction = () => builder.Parse(args);

            parseAction.Should().Throw<Exception>();
        }

        [Theory]
        [InlineData("2.0", "branch1")]
        [InlineData("1", "branch2")]
        public static void GivenAMixedTypeCommandValue_ShouldBeRedirected(string value, string branch)
        {
            var args = new[] { "-x", value };
            string? calledBranch = default;
            var builder = FluentArgsBuilder.New()
                .Given.Command("-x")
                    .HasValue(2.0).Then(() => calledBranch = "branch1")
                    .HasValue(1).Then(() => calledBranch = "branch2")
                    .ElseIsInvalid()
                .Call(() => calledBranch = "none");

            builder.Parse(args);

            calledBranch.Should().Be(branch);
        }

        //TODO: What if HasValue<T> fails to parse the value? Go to the nest HasValue or ignore it?

        [Theory]
        [InlineData("v1", "v2", "v1v2")]
        [InlineData("v1", "xx", "v1")]
        [InlineData("a", "b", "none")]
        [InlineData("a", "v2", "none")]
        public static void NestedGivenCommand_ShouldbeHandledCorrect(string command1, string command2, string expectedBranch)
        {
            var args = new[] { "-c1", command1, "-c2", command2 };
            string? calledBranch = default;
            var builder = FluentArgsBuilder.New()
                .Given.Command("-c1")
                    .HasValue("v1").Then(b => b
                        .Given.Command("-c2")
                            .HasValue("v2").Then(() => calledBranch = "v1v2")
                            .ElseIgnore()
                        .Call(() => calledBranch = "v1"))
                    .ElseIgnore()
                .Call(() => calledBranch = "none");

            builder.Parse(args);

            calledBranch.Should().Be(expectedBranch);
        }

        //TODO: serial nested with same command should not work (the command is removed from args)
        [Theory]
        [InlineData("v1", "v2", "v1")]
        [InlineData("v1", "xx", "v1")]
        [InlineData("xx", "v2", "v2")]
        [InlineData("xx", "xx", "none")]
        public static void SerialGivenCommand_ShouldBeHandledCorrect(string command1, string command2, string expectedBranch)
        {
            var args = new[] { "-c1", command1, "-c2", command2 };
            string? calledBranch = default;
            var builder = FluentArgsBuilder.New()
                .Given.Command("-c1")
                    .HasValue("v1").Then(() => calledBranch = "v1")
                    .ElseIgnore()
                .Given.Command("-c2")
                    .HasValue("v2").Then(() => calledBranch = "v2")
                    .ElseIgnore()
                .Call(() => calledBranch = "none");

            builder.Parse(args);

            calledBranch.Should().Be(expectedBranch);
        }

        public static void SerialGivenCommandWithTheSameCommand_ShouldIgnoreAllButTheFirstCall()
        {
            var args = new[] { "c1", "v1" };
            bool? redirected = default;
            var builder = FluentArgsBuilder.New()
                .Given.Command("-c1")
                    .HasValue("v1").Then(() => redirected = true)
                    .ElseIgnore()
                .Given.Command("-c1")
                    .HasValue("v2").Then(b => b.Invalid())
                    .ElseIgnore()
                .Call(() => redirected = false);

            builder.Parse(args);

            redirected.Should().BeTrue();
        }
    }
}
