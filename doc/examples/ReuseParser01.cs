namespace Example
{
    using System;
    using System.Threading.Tasks;
    using FluentArgs;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var parser = FluentArgsBuilder.New()
                .Parameter<int>("-n").IsRequired()
                .Call(n =>
                {
                    /*...*/
                    Console.WriteLine($"n={n}");
                    /*...*/
                    return Task.CompletedTask;
                })
                .Build();

            await parser.ParseAsync("-n", "1").ConfigureAwait(false);
            await parser.ParseAsync("-n", "2").ConfigureAwait(false);
            await parser.ParseAsync(args).ConfigureAwait(false);
        }
    }
}
