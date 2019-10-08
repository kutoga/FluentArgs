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
                .PositionalArgument().IsRequired()
                .PositionalArgument<int>().IsRequired()
                .PositionalArgument<bool>().IsOptionalWithDefault(false)
                .Call(p3 => p2 => p1 =>
                {
                    /* ... */
                    Console.WriteLine($"First parameter: {p1}");
                    Console.WriteLine($"Second parameter: {p2}");
                    Console.WriteLine($"Third parameter: {p3}");
                    /* ... */
                    return Task.CompletedTask;
                })
                .ParseAsync(args);
        }
    }
}

