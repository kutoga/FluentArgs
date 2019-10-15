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

                /* 1) General parser configurations: The ordering does not matter */
                .DefaultConfigs()
                .WithApplicationDescription("My app.")
                .WithAssignmentOperators("=", ":")
                .ThrowOnNonMinusStartingNames()
                .ThrowOnDuplicateNames()
                .ThrowIfUnusedArgumentsArePresent()
                .RegisterHelpFlag("-h")
                .RegisterHelpPrinter(new SimpleHelpPrinter(Console.Error))
                .RegisterParsingErrorPrinter(new SimpleParsingErrorPrinter(Console.Error))

                /* 2) Parse parameters, list-parameters, flags and commands. Parsing is done in the defined ordering. */
                .Parameter<int>("-n").IsRequired()
                .ListParameter<DateTimeOffset?>("-d").IsOptional()
                .Given.Command("-x")
                    .HasValue("y").Then(d => n => { })
                    .ElseIgnore()
                .Parameter("-o").IsOptional()

                /* 3) Positional parameters. Parsing is done in the defined ordering. */
                .PositionalArgument<int>().IsRequired()
                .PositionalArgument<float?>().IsOptional()

                /* 4) Load remaining arguments */
                .LoadRemainingArguments()

                /* 5) Callback */
                .Call(remainingArgs => floatArg => intArg => o => d => n =>
                {
                    Console.WriteLine("Hello World!");
                })
                .Parse(args);
        }
    }
}

