namespace Example
{
    using System;
    using FluentArgs;

    public static class Program
    {
        public static void Main(string[] args)
        {
            FluentArgsBuilder.New()
                .WithApplicationDescription("This applications shows how errors look like.")
                .RegisterHelpFlag("-h", "--help")
                .Parameter<int>("-n")
                    .WithDescription("A positive number.")
                    .WithExamples(1, 2, 100)
                    .WithValidation(n => n > 0, n => $"A positive number is required, but {n} is <= 0!")
                    .IsRequired()
                .Call(n => Console.WriteLine($"n={n}"))
                .Parse(args);
        }
    }
}

