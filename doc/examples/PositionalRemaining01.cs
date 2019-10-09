namespace Example
{
    using System;
    using System.Threading.Tasks;
    using FluentArgs;

    public static class Program
    {
        public static Task Main(string[] args)
        {
            return FluentArgsBuilder.New()
                .DefaultConfigsWithAppDescription("List files and / or subdirectories")
                .Parameter<char>("-t", "--type")
                    .WithDescription("List entry type (e.g. f=file, d=directory)")
                    .IsOptionalWithDefault('d')
                .PositionalArgument()
                    .WithDescription("The source directory")
                    .IsRequired()
                .Call(sourceDirectory => type =>
                {
                    /* ... */
                    Console.WriteLine($"Find all {type} filesystem entries in the directory {sourceDirectory}");
                    /* ... */
                    return Task.CompletedTask;
                })
                .ParseAsync(args);
        }
    }
}
