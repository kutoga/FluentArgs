namespace Example
{
    using FluentArgs;
    using System;
    using System.Threading.Tasks;

    class Program
    {
        public static Task Main(string[] args)
        {
            return FluentArgsBuilder.New()
                .Parameter<int>("-n").IsRequired()
                .Call(n => MyAsyncApp(n))
                .ParseAsync(args);
        }

        private static async Task MyAsyncApp(int n)
        {
            await Console.Out.WriteLineAsync($"n={n}").ConfigureAwait(false);
        }
    }
}
