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
                .Flag("-f")
                    .WithDescription("Force to delete a file")
                .LoadRemainingArguments()
                    .WithDescription("All files which should be deleted")
                .Call(files => f =>
                {
                    /* ... */
                    Console.WriteLine($"f-Flag: {f}");
                    Console.WriteLine($"Files: {string.Join(", ", files)}");
                    /* ... */
                    return Task.CompletedTask;
                })
                .ParseAsync(args);
        }
    }
}
