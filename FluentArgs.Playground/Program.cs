using FluentArgs.Help;

namespace FluentArgs.Playground
{
    using System;
    using System.Threading.Tasks;

    class Program
    {
        enum MyEnum
        {
            Pikachu,
            Randomon
        }

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
                    .WithValidator(n => n >= 0 && n <= 100)
                    .IsOptionalWithDefault(50)
                .Call(quality => outputFile => inputFile =>
                {
                    /* ... */
                    Console.WriteLine($"Convert {inputFile} to {outputFile} with qualiyt {quality}...");
                    /* ... */
                    return Task.CompletedTask;
                })
                .ParseAsync(args);
        }

        public static void Main2(string[] args)
        {
            FluentArgsBuilder.New()
                .Given.Command("-c", "--command")
                    .HasValue("copy").Then(_ => _
                        .Parameter("-s", "--source").IsRequired()
                        .Parameter("-t", "--target").IsRequired()
                        .Call(target => source =>
                        {
                            Console.WriteLine($"Copy from {source} to {target}...");
                        }))
                    .HasValue("delete").Then(_ => _
                        .LoadRemainingArguments()
                        .Call(files =>
                        {
                            /* ... */
                            Console.WriteLine($"Delete {string.Join(", ", files)}...");
                            /* ... */
                        }))
                    .ElseIgnore()
                .Invalid()
                .Parse(args);
        }

        static void Main3(string[] args)
        {
            //FluentArgsBuilder.New()
            //    .DefaultConfigs()
            //    .DefaultConfigsWithAppDescription("This application was just developed for testing purposes. It has no real-life application.")
            //    .Parameter<int>("-n", "--number")
            //        .WithDescription("Just a number")
            //        .IsOptional()
            //    .Parameter("-k", "--key")
            //        .WithDescription("A very secret key")
            //        .IsOptionalWithDefault("DEFAULT_KEY")
            //    .Parameter<MyEnum>("-e", "--e")
            //        .WithDescription("Choose your pokemon. This option is as useless as the others, but a long text is required to see if the line breaks work. Or not? Whatever.")
            //        .IsRequired()
            //    .Parameter("--name")
            //        .IsRequired()
            //    .ParameterList<MyEnum>("--drink")
            //        .WithDescription("What are your favourite drinks?")
            //        .IsOptional()
            //    .Parameter("--abc")
            //        .IsRequired()
            //    .Flag("-u")
            //        .WithDescription("just a flag...")
            //    .Flag("-w")
            //    .Given.Command("--special")
            //        .HasValue("xyz").Then(b => b
            //            .Parameter("-a")
            //                .IsRequired()
            //            .Given.Flag("--myflag").Then(b => b
            //                .Parameter("--x").IsOptional()
            //                .CallUntyped(d => { }))
            //            .CallUntyped(d =>
            //            {
            //            }))
            //        .ElseIgnore()
            //    .LoadRemainingArguments()
            //    .Call(args => w => u => abc => drinks => name => e => key => n =>
            //    {
            //        Console.WriteLine($"n={n}");
            //    })
            //    .Parse("--help");
            FluentArgsBuilder.New()
                .DefaultConfigs()
                .Parameter("-a").IsRequired()
                .Parameter("-b").IsRequired()
                .PositionalArgument().IsOptional()
                .Call(c => b => a =>
                {
                    Console.WriteLine($"a={a}");
                    Console.WriteLine($"b={b}");
                })
                .Parse("--help", "-a", "bla");
            Console.ReadLine();
            return;

            //TODO: Given.Command(...) => if the comand is not present there should be a possibility to define this as invalid (the command is required)

            //TODO: Call .Build().Parse(xxx) instead of .Parse(); BUT add an extension method or something which just does this for you
            //TODO: IsOptionalWithDefault(...) -> IsOptional().WithDefault(...); vielleicht

            Console.WriteLine("Hello World!");

            args = new[] { "-m", "-n", "beni" };

            FluentArgsBuilder.New()
                //.Parameter("-n", "--name").IsRequired()
                .Given.Flag("-m").Then(b => b
                    //   .Parameter("-n").IsRequired()
                    .Call(() => //name =>
                    {
                        Console.WriteLine($"Hello !");
                    }))
                .Call(() =>
                {
                    Console.WriteLine("Brauche -m parameter");
                })
                .Parse(args);

            Console.ReadLine();
            return;

            FluentArgsBuilder.New()
                .ParameterList<int>("-n", "--numbers").IsRequired()
                .Parameter("--name")
                    .WithDescription("")
                    .WithExamples("")
                    .IsRequired()
                .Call(name => numbers => { })
                .Parse(args);

            FluentArgsBuilder.New()
                .Given.Parameter("-c", "--command").HasValue("copy").Then(b => b
                    .Parameter("-i", "--input").IsRequired()
                    .Parameter<int>("-b", "--blocksize").IsOptional()
                    .Call(blockSize => input =>
                    {
                        return Task.CompletedTask;
                    }))
                .Call(() => throw new Exception("no flag given"))
                .Parse(args);

            FluentArgsBuilder.New()

                /* Help etc. */
                .Given.Flag("-h", "--help").Then(b => b.Call(() =>
                {
                    Console.WriteLine("Show help"); // put this to an extension method
                }))

                /* general settings / arguments */
                .Parameter("-k", "--apikey")
                    .WithDescription("the magic super expensive api key")
                    .WithExamples("ABC", "123")
                    .IsRequired()

                /* switch like command parameters */
                .Given.Command("-c", "--command")
                    .HasValue("copy").Then(b => b
                        .Parameter("-i", "--input").IsRequired()
                        .Parameter<int>("-b", "--blocksize").IsOptionalWithDefault(-1)
                        .Call(b => i => key =>
                        {

                        }))
                    .HasValue("delete").Then(b => b
                        .Parameter("-f", "--file").IsRequired()
                        .Call(file => key =>
                        {

                        }))
                    .ElseIgnore()

                .Given.Command("-x", "--execute")
                    .HasValue("now").Then(b => b.Call(key => Console.WriteLine("excute")))
                    .Matches<DateTimeOffset>(d => d != DateTimeOffset.Now, DateTimeOffset.Parse).Then(b => b.Call(Console.WriteLine))
                    .ElseIgnore()

                .Given.Command("-c")
                    .HasValue(1).Then(b => b
                        .Parameter<int>("u").IsRequired()
                        .Given.Command("-c2")
                            .HasValue(2).Then(b => b
                                .Parameter<double>("v").IsRequired()
                                .Call(v => u => key =>
                                {

                                }))
                            .ElseIsInvalid()
                        .Invalid())
                    .ElseIgnore()
                .Call(_ =>
                {
                    throw new Exception("blabla");
                    return;
                })
                .ParseAsync(args);


            /*
             *TODO:
             * .ConfigureDefaults() ->
 
.ThrowOnNonMinusStartingNames() // basiert auf RegisterOnBuiltCallback(Action<…>)
.WarnOnDuplicateUsedNames() // nur im selben branch
.ShowHelp("-h", "--help")
*/
        }
    }
}
