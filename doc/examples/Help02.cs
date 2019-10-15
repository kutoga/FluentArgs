namespace Example
{
    using System;
    using FluentArgs;
    using FluentArgs.Help;

    public static class Program
    {
        public static void Main(string[] args)
        {
            FluentArgsBuilder.New()
                .WithApplicationDescription("This application demonstrates how to use the help-features.")
                .RegisterHelpFlag("-h", "--help", "--another-help-flag")

                /* Inject a custom IHelpPrinter or use the pre-defined SimpleHelpPrinter */
                .RegisterHelpPrinter(new SimpleHelpPrinter(Console.Error))

                .Parameter("-n", "--name")
                    .WithDescription("Your name.")
                    .WithExamples("Peter", "Benjamin")
                    .IsRequired()
                .Parameter<int>("-a", "--age")
                    .WithDescription("Your age.")
                    .WithExamples(23, 56)
                    .WithValidation(a => a >= 0 && a <= 120, a => $"You are probably not {a} years old")
                    .IsRequired()
                .Parameter<string?>("-e", "--email")
                    .WithDescription("Your email address.")
                    .WithExamples("mrmojito@mymail.com", "me@cookislands.de")
                    .WithValidation(m => m.Contains('@'), "Your mail must contain an @-sign!")
                    .IsOptional()
                .Call(email => age => name =>
                {
                    Console.WriteLine($"Name: {name}");
                    Console.WriteLine($"Age: {age}");
                    Console.WriteLine($"EMail: {email}");
                })
                .Parse(args);
        }
    }
}

