namespace FluentArgs.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Execution;
    using FluentArgs.Help;

    internal class StepBuilder :
        IInitialFluentArgsBuilder
    {
        public Step Step { get; set; } = new InitialStep()
        {
            ParserSettings = new ParserSettings(new SimpleHelpPrinter(Console.Out), new SimpleParsingErrorPrinter(Console.Error))
            {
                AssignmentOperators = new[] { "=" }
            }
        };

        // TODO: public und interface prefix weg
        IGiven<IFluentArgsBuilder> IGivenAppliable<IFluentArgsBuilder>.Given =>
                new GivenBuilder<IFluentArgsBuilder>(() => new StepBuilder(), Step, s => new StepBuilder() { Step = s });

        public IBuildable Call(Action callback)
        {
            return new FinalBuilder(new CallStep(Step, new TargetFunction(callback)));
        }

        public IBuildable Call(Func<Task> callback)
        {
            return new FinalBuilder(new CallStep(Step, new TargetFunction(callback)));
        }

        public IBuildable CallUntyped(Action<IReadOnlyCollection<object?>> callback)
        {
            return new FinalBuilder(new UntypedCallStep(Step, new UntypedTargetFunction(callback)));
        }

        public IBuildable CallUntyped(Func<IReadOnlyCollection<object?>, Task> callback)
        {
            return new FinalBuilder(new UntypedCallStep(Step, new UntypedTargetFunction(callback)));
        }

        public IBuildable Invalid()
        {
            return new FinalBuilder(new InvalidStep(Step));
        }

        public IConfigurablePositionalArgument<IPositionalArgumentFluentArgsBuilder<Action<TParam>, Func<TParam, Task>>, TParam> PositionalArgument<TParam>()
        {
            var nextBuilder = new StepBuilder<Action<TParam>, Func<TParam, Task>>();
            return new PositionalArgumentBuilder<IPositionalArgumentFluentArgsBuilder<Action<TParam>, Func<TParam, Task>>, TParam>(
                PositionalArgumentBuilt, nextBuilder);

            void PositionalArgumentBuilt(PositionalArgument positionalArgument) =>
                nextBuilder.Step = new PositionalArgumentStep(Step, positionalArgument);
        }

        public IConfigurableRemainingArguments<Action<IReadOnlyList<TParam>>, Func<IReadOnlyList<TParam>, Task>, TParam> LoadRemainingArguments<TParam>()
        {
            return new RemainingArgumentsBuilder<Action<IReadOnlyList<TParam>>, Func<IReadOnlyList<TParam>, Task>, TParam>(
                s => new StepBuilder<Action<IReadOnlyList<TParam>>, Func<IReadOnlyList<TParam>, Task>> { Step = s }, Step);
        }

        public IInitialFluentArgsBuilder WithoutAssignmentOperators()
        {
            ((InitialStep)Step).ParserSettings!.AssignmentOperators = Array.Empty<string>();
            return this;
        }

        public IInitialFluentArgsBuilder RegisterHelpFlag(string name, params string[] moreNames)
        {
            ((InitialStep)Step).ParserSettings!.HelpFlag = Name.ValidateAndBuild(name, moreNames);
            return this;
        }

        public IInitialFluentArgsBuilder RegisterHelpPrinter(IHelpPrinter helpPrinter)
        {
            ((InitialStep)Step).ParserSettings!.HelpPrinter = helpPrinter;
            return this;
        }

        public IInitialFluentArgsBuilder RegisterParsingErrorPrinter(IParsingErrorPrinter parsingErrorPrinter)
        {
            ((InitialStep)Step).ParserSettings!.ParsingErrorPrinter = parsingErrorPrinter;
            return this;
        }

        public IInitialFluentArgsBuilder ThrowOnDuplicateNames(bool enable)
        {
            ((InitialStep)Step).ParserSettings!.ThrowOnDuplicateNames = enable;
            return this;
        }

        public IInitialFluentArgsBuilder ThrowOnNonMinusStartingNames(bool enable)
        {
            ((InitialStep)Step).ParserSettings!.ThrowOnNonMinusStartingNames = enable;
            return this;
        }

        public IInitialFluentArgsBuilder DisallowUnusedArguments(bool enable = true)
        {
            ((InitialStep)Step).ParserSettings!.DisallowUnusedArguments = enable;
            return this;
        }

        public IInitialFluentArgsBuilder WithApplicationDescription(string description)
        {
            ((InitialStep)Step).ParserSettings!.ApplicationDescription = description;
            return this;
        }

        public IInitialFluentArgsBuilder WithAssignmentOperators(string assignmentOperator, params string[] moreAssignmentOperators)
        {
            ((InitialStep)Step).ParserSettings!.AssignmentOperators = new[] { assignmentOperator }.Concat(moreAssignmentOperators).ToArray();
            return this;
        }

        IConfigurableFlagWithOptionalDescription IFluentArgsBuilder.Flag(string name, params string[] moreNames)
        {
            return new FlagBuilder(s => new StepBuilder<Action<bool>, Func<bool, Task>> { Step = s }, Step, new Flag(Name.ValidateAndBuild(name, moreNames)));
        }

        IConfigurableParameter<IFluentArgsBuilder<Action<TNextParam>, Func<TNextParam, Task>>, TNextParam> IFluentArgsBuilder.Parameter<TNextParam>(string name, params string[] moreNames)
        {
            var nextBuilder = new StepBuilder<Action<TNextParam>, Func<TNextParam, Task>>();
            return new ParameterBuilder<IFluentArgsBuilder<Action<TNextParam>, Func<TNextParam, Task>>, TNextParam>(
                ParameterBuilt, nextBuilder, Name.ValidateAndBuild(name, moreNames));

            void ParameterBuilt(Parameter parameter) =>
                nextBuilder.Step = new ParameterStep(Step, parameter);
        }

        IConfigurableListParameter<IFluentArgsBuilder<Action<IReadOnlyList<TNextParam>>, Func<IReadOnlyList<TNextParam>, Task>>, TNextParam> IFluentArgsBuilder.ListParameter<TNextParam>(string name, params string[] moreNames)
        {
            var nextBuilder = new StepBuilder<Action<IReadOnlyList<TNextParam>>, Func<IReadOnlyList<TNextParam>, Task>>();
            return new ListParameterBuilder<IFluentArgsBuilder<Action<IReadOnlyList<TNextParam>>, Func<IReadOnlyList<TNextParam>, Task>>, TNextParam>(
                ListParameterBuilt, nextBuilder, Name.ValidateAndBuild(name, moreNames));

            void ListParameterBuilt(ListParameter listParameter) =>
                nextBuilder.Step = new ListParameterStep(Step, listParameter);
        }
    }

    internal class StepBuilder<TFunc, TFuncAsync> : IFluentArgsBuilder<TFunc, TFuncAsync>
    {
        public Step Step { get; set; } = new InitialStep();

        public IGiven<IFluentArgsBuilder<TFunc, TFuncAsync>> Given =>
                new GivenBuilder<IFluentArgsBuilder<TFunc, TFuncAsync>>(
                    () => new StepBuilder<TFunc, TFuncAsync>(), Step, s => new StepBuilder<TFunc, TFuncAsync>() { Step = s });

        public IBuildable Call(TFunc callback)
        {
            return new FinalBuilder(new CallStep(Step, new TargetFunction(callback!)));
        }

        public IBuildable Call(TFuncAsync callback)
        {
            return new FinalBuilder(new CallStep(Step, new TargetFunction(callback!)));
        }

        public IBuildable CallUntyped(Action<IReadOnlyCollection<object?>> callback)
        {
            return new FinalBuilder(new UntypedCallStep(Step, new UntypedTargetFunction(callback)));
        }

        public IBuildable CallUntyped(Func<IReadOnlyCollection<object?>, Task> callback)
        {
            return new FinalBuilder(new UntypedCallStep(Step, new UntypedTargetFunction(callback)));
        }

        public IBuildable Invalid()
        {
            return new FinalBuilder(new InvalidStep(Step));
        }

        public IConfigurablePositionalArgument<IPositionalArgumentFluentArgsBuilder<Func<TNextParam, TFunc>, Func<TNextParam, TFuncAsync>>, TNextParam> PositionalArgument<TNextParam>()
        {
            var nextBuilder = new StepBuilder<Func<TNextParam, TFunc>, Func<TNextParam, TFuncAsync>>();
            return new PositionalArgumentBuilder<IPositionalArgumentFluentArgsBuilder<Func<TNextParam, TFunc>, Func<TNextParam, TFuncAsync>>, TNextParam>(
                PositionalArgumentBuilt, nextBuilder);

            void PositionalArgumentBuilt(PositionalArgument positionalArgument) =>
                nextBuilder.Step = new PositionalArgumentStep(Step, positionalArgument);
        }

        public IConfigurableRemainingArguments<Func<IReadOnlyList<TParam>, TFunc>, Func<IReadOnlyList<TParam>, TFuncAsync>, TParam> LoadRemainingArguments<TParam>()
        {
            return new RemainingArgumentsBuilder<Func<IReadOnlyList<TParam>, TFunc>, Func<IReadOnlyList<TParam>, TFuncAsync>, TParam>(
                s => new StepBuilder<Func<IReadOnlyList<TParam>, TFunc>, Func<IReadOnlyList<TParam>, TFuncAsync>> { Step = s }, Step);
        }

        public IConfigurableFlagWithOptionalDescription<TFunc, TFuncAsync> Flag(string name, params string[] moreNames)
        {
            return new FlagBuilder<TFunc, TFuncAsync>(s => new StepBuilder<Func<bool, TFunc>, Func<bool, TFuncAsync>> { Step = s }, Step, new Flag(Name.ValidateAndBuild(name, moreNames)));
        }

        public IConfigurableParameter<IFluentArgsBuilder<Func<TNextParam, TFunc>, Func<TNextParam, TFuncAsync>>, TNextParam> Parameter<TNextParam>(string name, params string[] moreNames)
        {
            // TODO: TNextParam -> TParam
            var nextBuilder = new StepBuilder<Func<TNextParam, TFunc>, Func<TNextParam, TFuncAsync>>();
            return new ParameterBuilder<IFluentArgsBuilder<Func<TNextParam, TFunc>, Func<TNextParam, TFuncAsync>>, TNextParam>(
                ParameterBuilt, nextBuilder, Name.ValidateAndBuild(name, moreNames));

            void ParameterBuilt(Parameter parameter) =>
                nextBuilder.Step = new ParameterStep(Step, parameter);
        }

        public IConfigurableListParameter<IFluentArgsBuilder<Func<IReadOnlyList<TNextParam>, TFunc>, Func<IReadOnlyList<TNextParam>, TFuncAsync>>, TNextParam> ListParameter<TNextParam>(string name, params string[] moreNames)
        {
            var nextBuilder = new StepBuilder<Func<IReadOnlyList<TNextParam>, TFunc>, Func<IReadOnlyList<TNextParam>, TFuncAsync>>();
            return new ListParameterBuilder<IFluentArgsBuilder<Func<IReadOnlyList<TNextParam>, TFunc>, Func<IReadOnlyList<TNextParam>, TFuncAsync>>, TNextParam>(
                ListParameterBuilt, nextBuilder, Name.ValidateAndBuild(name, moreNames));

            void ListParameterBuilt(ListParameter listParameter) =>
                nextBuilder.Step = new ListParameterStep(Step, listParameter);
        }
    }
}
