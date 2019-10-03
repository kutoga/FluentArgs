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
                .DefaultConfigs()
                .Parameter("-i", "--input").IsRequired()
                .Parameter("-o", "--output").IsRequired()
                .Parameter<ushort>("-q", "--quality")
                    .WithValidator(n => n >= 0 && n <= 100)
                    .IsOptionalWithDefault(50)
                .Call(quality => outputFile => inputFile =>
                {
                    /* ... */
                    Console.WriteLine($"Convert {inputFile} to {outputFile} with quality {quality}...");
                    /* ... */
                    return Task.CompletedTask;
                })
                .ParseAsync(args);
        }
    }
}

