namespace Example
{
    using System;
    using System.Linq;
    using FluentArgs;

    public static class Program
    {
        private static int BinaryNumberParser(string input)
        {
            if (input.Length == 0) { return 0; }
            var lastDigit = input.Last() == '0' ? 0 : input.Last() == '1' ? 1 : throw new FormatException();
            return lastDigit + (2 * BinaryNumberParser(input.Substring(0, input.Length - 1)));
        }

        public static void Main(string[] args)
        {
            FluentArgsBuilder.New()
                .DefaultConfigs()
                .Parameter<int>("-b", "--binaryNumber")
                    .WithDescription("A binary number which is greater or equal to 0 and smaller or equal to 100")
                    .WithParser(BinaryNumberParser)
                    .WithValidation(n => n >= 0 && n <= 100)
                    .IsRequired()
                .Call(n => Console.WriteLine($"Number: {n}"))
                .Parse(args);
        }
    }
}
