[![Build Status](https://dev.azure.com/benjaminmeier70/PipelinePlayground/_apis/build/status/kutoga.FluentArgs?branchName=master)](https://dev.azure.com/benjaminmeier70/PipelinePlayground/_build/latest?definitionId=3&branchName=master)

# FluentArgs: Fluent Arguments Parsing for .NET

FluentArgs is an easy-to-use library that provides command line argument parsing. For all parameters it is possible
to provide meta information (description, examples etc.) which might be used to auto-generate a simple help for the
final application.

# Why FluentArgs?

The API is optimized to be as readable and type-safe as possible. Therefore, anyone can learn how to use this library
in just a few minutes.

![](doc/test.gif)

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
!INCLUDE:examples/Simple01.cs
```

You might wonder why the order of parameters for the `Call`-method are inverted. This is due to a limitation
of the C#-programming language: If the order should be reversed, the number of parameters has to be limited
to a fixed number.

You also want to have a detailed help? Just annotate all parameters and call `myapp -h` or `myapp --help`.
The help flag is added by the `DefaultConfigs...`-call. As you can see later, it is possible to disable the
help flag, to use a different help flag name or to customize the help output. It is also possible use async
code.
```csharp
!INCLUDE:examples/Simple02.cs
```

# Example: Parse positional and remaining arguments
Positional arguments without an explicit name might be used if the context defines their meaning. E.g.
`find --type f ./my_directory` shall be parsed. An equivalent call is `find ./my_directory --type f`. The
source directory is a positional argument.

Such arguments can be defined after all simple arguments and flags are defined:
```csharp
!INCLUDE:examples/PositionalRemaining01.cs
```

It is no problem to define multiple positional arguments:
```csharp
!INCLUDE:examples/PositionalRemaining02.cs
```

It is also possible to parse all remaining arguments. E.g., if calls like `rm -f file1 file2 file` should
be supported (with an arbitrary number of files), this can be achieved by the following code:
```csharp
!INCLUDE:examples/PositionalRemaining03.cs
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
!INCLUDE:examples/Given01.cs
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
!INCLUDE:examples/Given02.cs
```

# Example: List parameters

# Example: Async vs Blocking
Both, async and blocking, calls are supported. An async example:
```csharp
!INCLUDE:examples/AsyncBlocking01.cs
```

A blocking example:
```csharp
!INCLUDE:examples/AsyncBlocking02.cs
```

# Example: Help

# Example: Handle errors

# Example: Advanced configuration

# Example: Reuse parser
It might be the case that you want to reuse a parser. In this case it is more efficient to explicit build
the internal tree with the `.Build()` method and use the resulting parser.
```csharp
!INCLUDE:examples/ReuseParser01.cs
```

# Best practices
E.g. just call one method in `Call`. etc.

