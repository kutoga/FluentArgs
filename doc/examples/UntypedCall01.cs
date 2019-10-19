namespace Example
{
    using System;
    using FluentArgs;

    public static class Program
    {
        public static void Main(string[] args)
        {
            FluentArgsBuilder.New()
                .Parameter("-a").IsRequired()
                .Parameter<int>("-b").IsRequired()
                .CallUntyped(args =>
                {
                    /* args is of the type IReadOnlyCollection<object?> */
                    /* args[0] is the value of the parameter "-a" */
                    /* args[1] is the value of the parameter "-b" */
                })
                .Parse(args);
        }
    }
}
