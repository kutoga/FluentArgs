namespace Example
{
    using System;

    using FluentArgs;

    public static class Program
    {
        public static void Main(string[] args)
        {
            FluentArgsBuilder.New()
                .ListParameter("--names")
                    .WithDescription("A list of names.")
                    .WithValidation(n => !string.IsNullOrWhiteSpace(n), "A name must not only contain whitespace.")
                    .IsRequired()
                .Call(names =>
                {
                    foreach (var name in names)
                    {
                        Console.WriteLine(name);
                    }
                })
                .Parse(args);
        }
    }
}

