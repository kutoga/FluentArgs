namespace Example
{
    using System;
    using FluentArgs;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var success = FluentArgsBuilder.New()
                .PositionalArgument<int>().IsRequired()
                .Call(_ => { })
                .Parse(args);
            Console.WriteLine($"Parse success: {success}");
        }
    }
}

