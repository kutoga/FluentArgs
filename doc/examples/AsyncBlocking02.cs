namespace Example
{
    using FluentArgs;
    using System;
    using System.Threading.Tasks;

    class Program
    {
        public static void Main(string[] args)
        {
            FluentArgsBuilder.New()
                .Parameter<int>("-n").IsRequired()
                .Call(n => MyBlockingApp(n))
                .Parse(args);
        }

        private static void MyBlockingApp(int n)
        {
            Console.WriteLine($"n={n}");
        }
    }
}
