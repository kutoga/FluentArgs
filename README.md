[![Build Status](https://dev.azure.com/benjaminmeier70/PipelinePlayground/_apis/build/status/kutoga.FluentArgs?branchName=master)](https://dev.azure.com/benjaminmeier70/PipelinePlayground/_build/latest?definitionId=3&branchName=master)

# FluentArgs: Fluent Arguments Parsing for .NET

FluentArgs is an easy-to-use library that provides command line argument parsing. For all parameters it is possible
to provide meta information (description, examples etc.) which might be used to auto-generate a simple help for the
final application.

# Why FluentArgs?

The API is optimized to be as readable as possible. Therefore, anyone can learn how to use this library
in just a few minutes.

![](doc/test.gif)

# How to install
TODO

# Example: Parse simple arguments and flags
Given you want a program which supports png to jpeg conversion and you want the support following calls:
- `myapp -i image.png -o image.jpeg -q 100`
- `myapp --input image.png --quality 50 --output image.jpeg`
- etc.

There's the code:
```csharp
namespace Example
{
    using FluentArgs;
    using System;
    using System.Threading.Tasks;

    class Program
    {
        public static Task Main(string[] args)
        {
            return FluentArgsBuilder.New()
                .DefaultConfigs()
                .Parameter("-i", "--input").IsRequired()
                .Parameter("-o", "--output").IsRequired()
                .Parameter<ushort>("-q", "--quality")
                    .WithValidation(n => n >= 0 && n <= 100)
                    .IsOptionalWithDefault(50)
                .Call(quality => outputFile => inputFile =>
                {
                    /* ... */
                    Console.WriteLine($"Convert {inputFile} to {outputFile} with quality {quality}...");
                    /* ... */
                    return Task.CompletedTask;
                })
                .ParseAsync(args);
        }
    }
}

```

You also want to have a detailed help? Just annotate all parameters and call `myapp -h` or `myapp --help`.
The help flag is added by the `DefaultConfigs...`-call. As you can see later, it is possible to disable the
help flag, to use a different help flag name or to customize the help output.
```csharp
namespace Example
{
    using FluentArgs;
    using System;
    using System.Threading.Tasks;

    class Program
    {
        public static Task Main(string[] args)
        {
            return FluentArgsBuilder.New()
                .DefaultConfigsWithAppDescription("An app to convert png files to jpg files.")
                .Parameter("-i", "--input")
                    .WithDescription("Input png file")
                    .WithExamples("input.png")
                    .IsRequired()
                .Parameter("-o", "--output")
                    .WithDescription("Output jpg file")
                    .WithExamples("output.jpg")
                    .IsRequired()
                .Parameter<ushort>("-q", "--quality")
                    .WithDescription("Quality of the conversion")
                    .WithValidation(n => n >= 0 && n <= 100)
                    .IsOptionalWithDefault(50)
                .Call(quality => outputFile => inputFile =>
                {
                    /* ... */
                    Console.WriteLine($"Convert {inputFile} to {outputFile} with quality {quality}...");
                    /* ... */
                    return Task.CompletedTask;
                })
                .ParseAsync(args);
        }
    }
}

```

# Example: Parse positional and remaining arguments

# Example: Parse conditional arguments / commands

# Example: Help

# Example: Advanced configuration

# Example: Reuse parser

# Best practices
E.g. just call one method in `Call`. etc.

