using System;
using System.Threading.Tasks;

namespace MyExample
{
    public static class Program
    {
        public static Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            return Task.CompletedTask;
        }
    }
}


