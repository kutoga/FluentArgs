namespace Example
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentArgs;
    using FluentArgs.Help;

    public static class Program
    {
        public static void Main(string[] args)
        {
            FluentArgsBuilder.New()

                .RegisterParsingErrorPrinter(new SimpleParsingErrorPrinter(Console.Out))
                /* or */
                .RegisterParsingErrorPrinter(new MyParsingErrorPrinter())

                .PositionalArgument<int>().IsRequired()
                .Call(_ => { })
                .Parse(args);
        }

        private class MyParsingErrorPrinter : IParsingErrorPrinter
        {
            public Task PrintArgumentMissingError(IReadOnlyCollection<string>? aliases, Type targetType, string description, IReadOnlyCollection<string>? helpFlagAliases)
            {
                throw new NotImplementedException();
            }

            public Task PrintArgumentParsingError(IReadOnlyCollection<string>? aliases, Type targetType, string description, IReadOnlyCollection<string>? helpFlagAliases)
            {
                throw new NotImplementedException();
            }

            public Task PrintInvalidCommandValueError(IReadOnlyCollection<string> aliases, string value, IReadOnlyCollection<string>? helpFlagAliases)
            {
                throw new NotImplementedException();
            }
        }
    }
}

