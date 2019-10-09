[![Build Status](https://dev.azure.com/benjaminmeier70/PipelinePlayground/_apis/build/status/kutoga.FluentArgs?branchName=master)](https://dev.azure.com/benjaminmeier70/PipelinePlayground/_build/latest?definitionId=3&branchName=master)

# FluentArgs: Fluent Arguments Parsing for .NET

FluentArgs is an easy-to-use library that provides command line argument parsing. For all parameters it is possible
to provide meta information (description, examples etc.) which might be used to auto-generate a simple help for the
final application.

# Why FluentArgs?

The API is optimized to be as readable and type-safe as possible. Therefore, anyone can learn how to use this library
in just a few minutes.

![](doc/gif-example/example.gif)

# How to install
TODO

TODO: Am Anfang beschreiben, dass immer COnfigureWithDefaults gemacht wird und das empfohlen wird

# Example: Parse simple arguments and flags
Given you want a program which supports png to jpeg conversion and you want to support calls like these:
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
        public static void Main(string[] args)
        {
            FluentArgsBuilder.New()
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
                })
                .Parse(args);
        }
    }
}

```

You might wonder why the order of parameters for the `Call`-method are inverted. This is due to a limitation
of the C#-programming language: If the order should be reversed, the number of parameters has to be limited
to a fixed number.

You also want to have a detailed help? Just annotate all parameters and call `myapp -h` or `myapp --help`.
The help flag is added by the `DefaultConfigs...`-call. As you can see later, it is possible to disable the
help flag, to use a different help flag name or to customize the help output. It is also possible use async
code.
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
Positional arguments without an explicit name might be used if the context defines their meaning. E.g.
`find --type f ./my_directory` shall be parsed. An equivalent call is `find ./my_directory --type f`. The
source directory is a positional argument.

Such arguments can be defined after all simple arguments and flags are defined:
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

```

It is no problem to define multiple positional arguments:
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
                .PositionalArgument().IsRequired()
                .PositionalArgument<int>().IsRequired()
                .PositionalArgument<bool>().IsOptionalWithDefault(false)
                .Call(p3 => p2 => p1 =>
                {
                    /* ... */
                    Console.WriteLine($"First parameter: {p1}");
                    Console.WriteLine($"Second parameter: {p2}");
                    Console.WriteLine($"Third parameter: {p3}");
                    /* ... */
                    return Task.CompletedTask;
                })
                .ParseAsync(args);
        }
    }
}

```

It is also possible to parse all remaining arguments. E.g., if calls like `rm -f file1 file2 file` should
be supported (with an arbitrary number of files), this can be achieved by the following code:
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
```

# Example: Parse conditional arguments / commands
Conditional arguments allow to control the argument parsing flow. E.g., the reuqirements for our CLI is
the following:
- If the flag `-v` (or `--version`) is given, the program version should be print (independent of all other parameters)
- If the flag `-u` (or `--update`) is given, an update should be downloaded and installed
  - An optional parameter `-s` (or `--source`) defines the update source
- Otherwise the program takes it first two positional arguments and prints their sum: `myapp 1 2` should print `1+2=3`

The following code fullfills this specifications:
```csharp
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
                .WithApplicationDescription("A simple calculator: add two numbers")
                .Given.Flag("-v", "--version").Then(() =>
                {
                    /*...*/
                    Console.WriteLine("Program version: 2.0");
                    /*...*/
                })
                .Given.Flag("-u", "--update").Then(b => b
                    .Parameter<Uri>("-s", "--source")
                        .WithDescription("Update source url")
                        .IsOptionalWithDefault(new Uri("http://my-update-server.com/update.zip"))
                    .Call(uri =>
                    {
                        /*...*/
                        Console.WriteLine($"Install update from {uri}...");
                        /*...*/
                    }))
                .PositionalArgument<int>()
                    .WithDescription("The first number")
                    .IsRequired()
                .PositionalArgument<int>()
                    .WithDescription("the second number")
                    .IsRequired()
                .Call(n2 => n1 =>
                {
                    /*...*/
                    Console.WriteLine($"{n1}+{n2}={n1 + n2}");
                    /*...*/
                })
                .Parse(args);
        }
    }
}
```

Assuming you want to create an application to do some file system actions on a remote system. It is required
to use a key to access this remote system. There are "commands" for this file system which lead to the following
calls:
- `myapp --apykey key --command init`
- `myapp --apikey key --command delete --file /myfile`
- `myapp --apikey key --command move --source /opt/source --destination /opt/target`

Becaused all these operations may take long, an additional parameter `--timeout` can be added to each of the
defined calls.

If `--command` has an unknown value or does not exsit, the program call is invalid.

This parameter parsing specification can be implemented with the following code:
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
                .DefaultConfigsWithAppDescription("This app allows to access a remote file system and to execute some commands on it.")
                .Parameter("-k", "--apikey")
                    .WithDescription("The api key")
                    .IsRequired()
                .Parameter<uint>("-t", "--timeout")
                    .WithDescription("Command timeout in seconds")
                    .IsOptionalWithDefault(60)
                .Given.Command("-c", "--command")
                    .HasValue("init").Then(timeout => apiKey => Init(apiKey, timeout))
                    .HasValue("delete").Then(b => b
                        .Parameter("-f", "--file")
                            .WithDescription("The file to delete")
                            .IsRequired()
                        .Call(file => timeout => apiKey => Delete(apiKey, file, timeout)))
                    .HasValue("move").Then(b => b
                        .Parameter("-s", "--source")
                            .WithDescription("Source path")
                            .IsRequired()
                        .Parameter("-d", "--destination")
                            .WithDescription("Destination path")
                            .IsRequired()
                        .Call(destination => source => timeout => apiKey => Move(apiKey, source, destination, timeout)))
                    .ElseIsInvalid()
                .Invalid()
                .ParseAsync(args);
        }

        private static async Task Init(string apiKey, uint timeout)
        {
            /*...*/
            await Console.Out.WriteAsync($"Init: {nameof(apiKey)}={apiKey}, {nameof(timeout)}={timeout}").ConfigureAwait(false);
            /*...*/
        }

        private static async Task Delete(string apiKey, string file, uint timeout)
        {
            /*...*/
            await Console.Out.WriteAsync($"Delete: {nameof(apiKey)}={apiKey}, {nameof(file)}={file}, {nameof(timeout)}={timeout}").ConfigureAwait(false);
            /*...*/
        }

        private static async Task Move(string apiKey, string source, string target, uint timeout)
        {
            /*...*/
            await Console.Out.WriteAsync($"Move: {nameof(apiKey)}={apiKey}, {nameof(source)}={source}, {nameof(target)}={target}, {nameof(timeout)}={timeout}").ConfigureAwait(false);
            /*...*/
        }
    }
}
```

# Example: List parameters

# Example: Async vs Blocking
Both, async and blocking, calls are supported. An async example:
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
                .Parameter<int>("-n").IsRequired()
                .Call(n => MyAsyncApp(n))
                .ParseAsync(args);
        }

        private static async Task MyAsyncApp(int n)
        {
            await Console.Out.WriteLineAsync($"n={n}").ConfigureAwait(false);
        }
    }
}
```

A blocking example:
```csharp
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
```

# Example: Help

# Example: Handle errors

# Example: Advanced configuration

# Example: Reuse parser
It might be the case that you want to reuse a parser. In this case it is more efficient to explicit build
the internal tree with the `.Build()` method and use the resulting parser.
```csharp
namespace Example
{
    using FluentArgs;
    using System;
    using System.Threading.Tasks;

    class Program
    {
        public static async Task Main(string[] args)
        {
            var parser = FluentArgsBuilder.New()
                .Parameter<int>("-n").IsRequired()
                .Call(n =>
                {
                    /*...*/
                    Console.WriteLine($"n={n}");
                    /*...*/
                    return Task.CompletedTask;
                })
                .Build();

            await parser.ParseAsync("-n", "1").ConfigureAwait(false);
            await parser.ParseAsync("-n", "2").ConfigureAwait(false);
            await parser.ParseAsync(args).ConfigureAwait(false);
        }
    }
}
```

# Best practices
E.g. just call one method in `Call`. etc.

