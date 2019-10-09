namespace Example
{
    using System;
    using System.Threading.Tasks;
    using FluentArgs;

    public static class Program
    {
        public static Task Main(string[] args)
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
                        .Call(file => timeout => apiKey => Delete(apiKey, file, timeout)))
                    .HasValue("move").Then(b => b
                        .Parameter("-s", "--source")
                            .WithDescription("Source path")
                            .IsRequired()
                        .Parameter("-d", "--destination")
                            .WithDescription("Destination path")
                            .IsRequired()
                        .Call(destination => source => timeout => apiKey => Move(apiKey, source, destination, timeout)))
                    .ElseIsInvalid()
                .Invalid()
                .ParseAsync(args);
        }

        private static async Task Init(string apiKey, uint timeout)
        {
            /*...*/
            await Console.Out.WriteAsync($"Init: {nameof(apiKey)}={apiKey}, {nameof(timeout)}={timeout}").ConfigureAwait(false);
            /*...*/
        }

        private static async Task Delete(string apiKey, string file, uint timeout)
        {
            /*...*/
            await Console.Out.WriteAsync($"Delete: {nameof(apiKey)}={apiKey}, {nameof(file)}={file}, {nameof(timeout)}={timeout}").ConfigureAwait(false);
            /*...*/
        }

        private static async Task Move(string apiKey, string source, string target, uint timeout)
        {
            /*...*/
            await Console.Out.WriteAsync($"Move: {nameof(apiKey)}={apiKey}, {nameof(source)}={source}, {nameof(target)}={target}, {nameof(timeout)}={timeout}").ConfigureAwait(false);
            /*...*/
        }
    }
}
