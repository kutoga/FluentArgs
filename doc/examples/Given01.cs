namespace Example
{
    using System;
    using System.Threading.Tasks;
    using FluentArgs;

    public static class Program
    {
        public static void Main(string[] args)
        {
            FluentArgsBuilder.New()
                .WithApplicationDescription("A simple calculator: add two numbers")
                .Given.Flag("-v", "--version").Then(() =>
                {
                    /*...*/
                    Console.WriteLine("Program version: 2.0");
                    /*...*/
                })
                .Given.Flag("-u", "--update").Then(b => b
                    .Parameter<Uri>("-s", "--source")
                        .WithDescription("Update source url")
                        .IsOptionalWithDefault(new Uri("http://my-update-server.com/update.zip"))
                    .Call(uri =>
                    {
                        /*...*/
                        Console.WriteLine($"Install update from {uri}...");
                        /*...*/
                    }))
                .PositionalArgument<int>()
                    .WithDescription("The first number")
                    .IsRequired()
                .PositionalArgument<int>()
                    .WithDescription("the second number")
                    .IsRequired()
                .Call(n2 => n1 =>
                {
                    /*...*/
                    Console.WriteLine($"{n1}+{n2}={n1 + n2}");
                    /*...*/
                })
                .Parse(args);
        }
    }
}
