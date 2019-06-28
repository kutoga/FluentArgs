# FluentArgs V0.0.1-alpha

**Very important: This repo is currently in a very instable state. Almost nothing
is coded; therefore still much work has to be done (and will be done in the next
days).**


Argument parsing is a quite usual task. There are tons of libraries out there, which already
do it great. This library tries to solve this problem in a very fluent and type-safe way.

Assuming you have a CLI whcih allows to use these parameters: `xyz -f file -n number --key apikey`

This can be implemented in this way:

```cs
    class Program
    {
        static void Main(string[] args)
        {
            FluentArgs.New()
                .Parameter<string>("-f", "--file", "--anotheralias").IsRequired()
                .Parameter<int>("-n", "--number").IsOptionalWithDefault(999)
                .Parameter<string>("-k", "--key").IsRequired()
                .Call(key => number => file =>
                {
                    // file is string
                    // number is int
                    // key is string
                })
                .Parse(args);
        }
    }
```

In a second way, you also might like to add more meta-data to the parameters:

```cs
    class Program
    {
        static void Main(string[] args)
        {
            FluentArgs.New()
                .Parameter<string>("-f", "--file", "--anotheralias")
                    .WithDescription("Input file")
                    .WithExamples("my_file.txt", "another_file.txt")
                    .IsRequired()
                .Parameter<int>("-n", "--number")
                    .WithDescription("Just a test parameter")
                    .IsOptionalWithDefault(999)
                .Parameter<string>("-k", "--key")
                    .WithDescription("An API key")
                    .IsRequired()
                .Call(key => number => file =>
                {
                    // file is string
                    // number is int
                    // key is string
                })
                .Parse(args);
        }
    }
```

Given you have a more complex tool, that supports different *operations*. E.g. it can copy or delete
a file. A secret API key is required to do this. For deleting an optional timeout is seconds can be defined
(this doesnt make sense; it should just demonstrate the api).
Possible calls are:

`xyz -c copy -k API_KEY --source source_file.txt --target target_file.txt`

`xyz -c delete -k API_KEY --file my_file.txt --timeout 10`

The important point is, that some parameters are only required **given** other parameters. The described
configuration can be conducted with this code:

```cs
    class Program
    {
        static void Main(string[] args)
        {
            FluentArgs.New()
                .Parameter<string>("-k", "--key")
                    .WithDescription("API key")
                    .IsRequired()
                .Given.Command("-c", "--command")
                    .HasValue("move").Then(b => b
                        .Parameter<string>("-s", "--source")
                            .WithDescription("Source file.")
                            .IsRequired()
                        .Parameter<string>("-t", "--target")
                            .WithDescription("Target file.")
                            .IsRequired()
                        .Call(target => source => key =>
                        {
                            // key is string
                            // source is string
                            // target is string
                        }))
                    .HasValue("delete").Then(b => b
                        .Parameter<string>("-f", "--file")
                            .WithDescription("The file to delete")
                            .IsRequired()
                        .Parameter<int?>("-t", "--timeout")
                            .WithDescription("Timeout in seconds")
                            .IsOptional()
                        .Call(timeout => file => key =>
                        {
                            // key is string
                            // file is string
                            // timeout is int?
                        }))
                    .ElseIsInvalid()
                .Parse(args)
        }
    }
```
