namespace FluentArgs.Test.Examples
{
    using System;
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
            argumentParser.Parse(new[] { "--apikey", "secret", "--action", "copy", "--source", "/source/file", "--target", "/target/file" });

            dummyClient.ApiKey.Should().Be("secret");
            dummyClient.CopyFileCalls.Should().BeEquivalentTo(("/source/file", "/target/file"));
        }

        [Fact]
        public void TestDelete()
        {
            argumentParser.Parse(new[] { "-k", "secret", "--act", "delete", "--file", "/file" });

            dummyClient.ApiKey.Should().Be("secret");
            dummyClient.DeleteFileCalls.Should().BeEquivalentTo("/file");
        }

        [Fact]
        public void TestReset()
        {
            argumentParser.Parse(new[] { "--apikey", "secret", "-a", "reset" });

            dummyClient.ApiKey.Should().Be("secret");
            dummyClient.ResetAccountCalls.Should().BeEquivalentTo(new object[] { null });
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
