namespace FluentArgs.Test.Examples
{
    using System;
    using FluentArgs.Test.Helpers;
    using FluentAssertions;
    using Xunit;

    public class BasicWithoutMetaData
    {
        private readonly DummyClient dummyClient;
        private readonly IParsable argumentParser;

        public BasicWithoutMetaData()
        {
            dummyClient = new DummyClient();
            argumentParser = BuildParser(dummyClient);
        }

        [Fact]
        public void TestCopy()
        {
            var parseSuccess = argumentParser.Parse(new[] { "--apikey", "secret", "--action", "copy", "--source", "/source/file", "--target", "/target/file" });

            parseSuccess.Should().BeTrue();
            dummyClient.ApiKey.Should().Be("secret");
            dummyClient.CopyFileCalls.Should().BeEquivalentWithSameOrdering(("/source/file", "/target/file"));
        }

        [Fact]
        public void TestDelete()
        {
            var parseSuccess = argumentParser.Parse(new[] { "-k", "secret", "--act", "delete", "--file", "/file" });

            parseSuccess.Should().BeTrue();
            dummyClient.ApiKey.Should().Be("secret");
            dummyClient.DeleteFileCalls.Should().BeEquivalentWithSameOrdering("/file");
        }

        [Fact]
        public void TestReset()
        {
            var parseSuccess = argumentParser.Parse(new[] { "--apikey", "secret", "-a", "reset" });

            parseSuccess.Should().BeTrue();
            dummyClient.ApiKey.Should().Be("secret");
            dummyClient.ResetAccountCalls.Should().BeEquivalentWithSameOrdering(new object[] { null });
        }

        [Fact]
        public void TestInvalidCommand()
        {
            Action parseAction = () => argumentParser.Parse(new[] { "--apikey", "secret", "-a", "not_existing_command" });

            parseAction.Should().Throw<Exception>();
        }

        //TODO: Enum parser
        private static IParsable BuildParser(DummyClient dummyClient)
        {
            return FluentArgsBuilder.New()
                .Parameter("--apikey", "-k").IsRequired()
                .Given.Command("--action", "--act", "-a")
                    .HasValue("copy").Then(b => b
                        .Parameter("--source", "-s").IsRequired()
                        .Parameter("--target", "-t").IsRequired()
                        .Call(target => source => apiKey =>
                        {
                            dummyClient.ApiKey = apiKey;
                            dummyClient.CopyFile(source, target);
                        }))
                    .HasValue("delete", "del").Then(b => b
                        .Parameter("--file", "-f").IsRequired()
                        .Call(file => apiKey =>
                        {
                            dummyClient.ApiKey = apiKey;
                            dummyClient.DeleteFile(file);
                        }))
                    .HasValue("reset").Then(b => b
                        .Parameter<int?>("--timeout", "-t").IsOptional()
                        .Call(timeout => apiKey =>
                        {
                            dummyClient.ApiKey = apiKey;
                            dummyClient.ResetAccount(timeout);
                        }))
                    .ElseIsInvalid()
                .Invalid()
                .Build();
        }
    }
}
