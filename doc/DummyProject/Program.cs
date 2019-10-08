namespace Example
{
    using FluentArgs;
    using System;
    using System.Threading.Tasks;

    class Program
    {
        public static async Task Main(string[] args)
        {
            return FluentArgsBuilder.New()
                .DefaultConfigsWithAppDescription("This app allows to access a remote file system and to execute some commands on it.")
                .Parameter("-k", "--apikey")
                    .WithDescription("The api key")
                    .IsRequired()
                .Parameter<uint>("-t", "--timeout")
                    .WithDescription("Command timeout in seconds")
                    .IsOptionalWithDefault(60)
                .Given.Command("-c", "--command")
                    .HasValue("init").Then(timeout => apiKey => Init(apiKey, timeout))
                    .HasValue("delete").Then(b => b
                        .Parameter("-f", "--file")
                            .WithDescription("The file to delete")
                            .IsRequired()
                        .Call(file => timeout => key => Delete(apiKey, file, timeout))
        }

        private static async Task Init(string apiKey, uint timeout)
        {

        }

        private static async Task Delete(string apiKey, string file, uint timeout)
        {

        }

        private static async Task Move(string apiKey, string source, string target, uint timeout)
        {

        }
    }
}
