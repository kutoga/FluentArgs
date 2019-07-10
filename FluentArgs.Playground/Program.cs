﻿namespace FluentArgs.Playground
{
    using System;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
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
                .Call(name => numbers =>
                {

                })
                .Parse(args);

            FluentArgsBuilder.New()
                .Given.Parameter("-c", "--command").WithValue("copy").Then(b => b
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
 
.WarnOnNonMinusStartingNames() // basiert auf RegisterOnBuiltCallback(Action<…>)
.WarnOnDuplicateUsedNames() // nur im selben branch
.ShowHelp("-h", "--help")
*/
        }
    }
}
