namespace FluentArgs.Builder
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Execution;
    using FluentArgs.Help;

    internal class StepBuilder : IInitialFluentArgsBuilder
    {
        public Step Step { get; set; } = new InitialStep()
        {
            ParserSettings = new ParserSettings(new SimpleHelpPrinter(Console.Out, Console.Error))
        };

        //TODO: public und interface prefix weg
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

        public IBuildable Invalid()
        {
            return new FinalBuilder(new InvalidStep(Step));
        }

        public IConfigurableRemainingArguments<Action<IReadOnlyList<TParam>>, Func<IReadOnlyList<TParam>, Task>, TParam> LoadRemainingArguments<TParam>()
        {
            return new RemainingArgumentsBuilder<Action<IReadOnlyList<TParam>>, Func<IReadOnlyList<TParam>, Task>, TParam>(
                s => new StepBuilder<Action<IReadOnlyList<TParam>>, Func<IReadOnlyList<TParam>, Task>> { Step = s }, Step);
        }

        public IInitialFluentArgsBuilder RegisterHelpFlag(string name, params string[] moreNames)
        {
            throw new NotImplementedException();
        }

        public IInitialFluentArgsBuilder RegisterHelpPrinter(IHelpPrinter helpPrinter)
        {
            ((InitialStep)Step).ParserSettings.HelpPrinter = helpPrinter;
            return this;
        }

        public IInitialFluentArgsBuilder WarnOnDuplicateNames()
        {
            ((InitialStep)Step).ParserSettings.WarnOnDuplicateNames = true;
            return this;
        }

        public IInitialFluentArgsBuilder WarnOnNonMinusStartingNames()
        {
            ((InitialStep)Step).ParserSettings.WarnOnNonMinusStartingNames = true;
            return this;
        }

        public IInitialFluentArgsBuilder WithApplicationDescription(string description)
        {
            ((InitialStep)Step).ParserSettings.ApplicationDescription = description;
            return this;
        }

        IConfigurableFlagWithOptionalDescription IFluentArgsBuilder.Flag(string name, params string[] moreNames)
        {
            return new FlagBuilder(s => new StepBuilder<Action<bool>, Func<bool, Task>> { Step = s }, Step, new Flag(new Name(name, moreNames)));
        }

        IConfigurableParameter<IFluentArgsBuilder<Action<TNextParam>, Func<TNextParam, Task>>, TNextParam> IFluentArgsBuilder.Parameter<TNextParam>(string name, params string[] moreNames)
        {
            var nextBuilder = new StepBuilder<Action<TNextParam>, Func<TNextParam, Task>>();
            return new ParameterBuilder<IFluentArgsBuilder<Action<TNextParam>, Func<TNextParam, Task>>, TNextParam>(
                ParameterBuilt, nextBuilder, new Name(name, moreNames));

            void ParameterBuilt(Parameter parameter) =>
                nextBuilder.Step = new ParameterStep(Step, parameter);
        }

        IConfigurableParameterList<IFluentArgsBuilder<Action<IReadOnlyList<TNextParam>>, Func<IReadOnlyList<TNextParam>, Task>>, TNextParam> IFluentArgsBuilder.ParameterList<TNextParam>(string name, params string[] moreNames)
        {
            var nextBuilder = new StepBuilder<Action<IReadOnlyList<TNextParam>>, Func<IReadOnlyList<TNextParam>, Task>>();
            return new ParameterListBuilder<IFluentArgsBuilder<Action<IReadOnlyList<TNextParam>>, Func<IReadOnlyList<TNextParam>, Task>>, TNextParam>(
                ParameterListBuilt, nextBuilder, new Name(name, moreNames));

            void ParameterListBuilt(ParameterList parameterList) =>
                nextBuilder.Step = new ParameterListStep(Step, parameterList);
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
            return new FinalBuilder(new CallStep(Step, new TargetFunction(callback)));
        }

        public IBuildable Call(TFuncAsync callback)
        {
            return new FinalBuilder(new CallStep(Step, new TargetFunction(callback)));
        }

        public IBuildable Invalid()
        {
            return new FinalBuilder(new InvalidStep(Step));
        }

        public IConfigurableRemainingArguments<Func<IReadOnlyList<TParam>, TFunc>, Func<IReadOnlyList<TParam>, TFuncAsync>, TParam> LoadRemainingArguments<TParam>()
        {
            return new RemainingArgumentsBuilder<Func<IReadOnlyList<TParam>, TFunc>, Func<IReadOnlyList<TParam>, TFuncAsync>, TParam>(
                s => new StepBuilder<Func<IReadOnlyList<TParam>, TFunc>, Func<IReadOnlyList<TParam>, TFuncAsync>> { Step = s }, Step);
        }

        IConfigurableFlagWithOptionalDescription<TFunc, TFuncAsync> IFluentArgsBuilder<TFunc, TFuncAsync>.Flag(string name, params string[] moreNames)
        {
            return new FlagBuilder<TFunc, TFuncAsync>(s => new StepBuilder<Func<bool, TFunc>, Func<bool, TFuncAsync>> { Step = s }, Step, new Flag(new Name(name, moreNames)));
        }

        IConfigurableParameter<IFluentArgsBuilder<Func<TNextParam, TFunc>, Func<TNextParam, TFuncAsync>>, TNextParam> IFluentArgsBuilder<TFunc, TFuncAsync>.Parameter<TNextParam>(string name, params string[] moreNames)
        {
            var nextBuilder = new StepBuilder<Func<TNextParam, TFunc>, Func<TNextParam, TFuncAsync>>();
            return new ParameterBuilder<IFluentArgsBuilder<Func<TNextParam, TFunc>, Func<TNextParam, TFuncAsync>>, TNextParam>(
                ParameterBuilt, nextBuilder, new Name(name, moreNames));

            void ParameterBuilt(Parameter parameter) =>
                nextBuilder.Step = new ParameterStep(Step, parameter);
        }

        IConfigurableParameterList<IFluentArgsBuilder<Func<IReadOnlyList<TNextParam>, TFunc>, Func<IReadOnlyList<TNextParam>, TFuncAsync>>, TNextParam> IFluentArgsBuilder<TFunc, TFuncAsync>.ParameterList<TNextParam>(string name, params string[] moreNames)
        {
            var nextBuilder = new StepBuilder<Func<IReadOnlyList<TNextParam>, TFunc>, Func<IReadOnlyList<TNextParam>, TFuncAsync>>();
            return new ParameterListBuilder<IFluentArgsBuilder<Func<IReadOnlyList<TNextParam>, TFunc>, Func<IReadOnlyList<TNextParam>, TFuncAsync>>, TNextParam>(
                ParameterListBuilt, nextBuilder, new Name(name, moreNames));

            void ParameterListBuilt(ParameterList parameterList) =>
                nextBuilder.Step = new ParameterListStep(Step, parameterList);
        }
    }
}
