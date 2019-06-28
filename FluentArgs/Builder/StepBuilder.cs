namespace FluentArgs.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentArgs.Description;
    using FluentArgs.Execution;

    internal class StepBuilder : IFluentArgsBuilder
    {
        public Step Step { get; set; } = new InitialStep();

        IGiven<IFluentArgsBuilder> IGivenAppliable<IFluentArgsBuilder>.Given =>
                new GivenBuilder<IFluentArgsBuilder>(new StepBuilder(), Step, s => new StepBuilder() { Step = s });

        public IParsable Call(Action callback)
        {
            var finalStep = new CallStep(Step, new TargetFunction(callback));
            return new StepCaller(finalStep);
        }

        public IParsable Call(Func<Task> callback)
        {
            var finalStep = new CallStep(Step, new TargetFunction(callback));
            return new StepCaller(finalStep);
        }

        public IConfigurableParameter<IFluentArgsBuilder<bool>, bool> Flag(string name, params string[] moreNames)
        {
            throw new NotImplementedException();
        }

        public IConfigurableParameter<IFluentArgsBuilder<TParam1>, TParam1> Parameter<TParam1>(string name, params string[] moreNames)
        {
            var nextBuilder = new StepBuilder<TParam1>();
            return new ParameterBuilder<IFluentArgsBuilder<TParam1>, TParam1>(ParameterBuilt, nextBuilder, new Name(name, moreNames));

            void ParameterBuilt(Parameter parameter)
            {
                nextBuilder.Step = new ParameterStep(Step, parameter);
            }
        }

        public IConfigurableParameter<IFluentArgsBuilder<IReadOnlyList<P1>>, P1> ParameterList<P1>(string name, params string[] moreNames)
        {
            throw new Exception();
            //var nextBuilder = new StepBuilder<IReadOnlyList<P1>>();
            //return new ParameterListBuilder<IFluentArgsBuilder<IReadOnlyList<P1>>, P1>(ParameterListBuilt, nextBuilder, new Name(name, moreNames));

            //void ParameterListBuilt(ParameterList parameterList)
            //{
            //}
        }

        public void Parse(string[] args)
        {
            throw new NotImplementedException();
        }

        public Task ParseAsync(string[] args)
        {
            throw new NotImplementedException();
        }
    }

    internal class StepBuilder<TParam1> : IFluentArgsBuilder<TParam1>
    {
        public Step Step { get; set; }

        IGiven<IFluentArgsBuilder<TParam1>> IGivenAppliable<IFluentArgsBuilder<TParam1>>.Given => throw new NotImplementedException();

        public IParsable Call(Action<TParam1> callback)
        {
            var finalStep = new CallStep(Step, new TargetFunction(callback));
            return new StepCaller(finalStep);
        }

        public IParsable Call(Func<TParam1, Task> callback)
        {
            var finalStep = new CallStep(Step, new TargetFunction(callback));
            return new StepCaller(finalStep);
        }

        public IConfigurableParameter<IFluentArgsBuilder<TParam1, bool>, bool> Flag(string name, params string[] moreNames)
        {
            throw new NotImplementedException();
        }

        public IFluentArgsBuilder<TParam1> IsOptional()
        {
            throw new NotImplementedException();
        }

        public IFluentArgsBuilder<TParam1> IsOptionalWithDefault(TParam1 defaultValue)
        {
            throw new NotImplementedException();
        }

        public IFluentArgsBuilder<TParam1> IsRequired()
        {
            throw new NotImplementedException();
        }

        public IConfigurableParameter<IFluentArgsBuilder<TParam1, P2>, P2> Parameter<P2>(string name, params string[] moreNames)
        {
            throw new NotImplementedException();
        }

        public IConfigurableParameter<IFluentArgsBuilder<TParam1, IReadOnlyList<P2>>, P2> ParameterList<P2>(string name, params string[] moreNames)
        {
            throw new NotImplementedException();
        }

        public void Parse(string[] args)
        {
            throw new NotImplementedException();
        }

        public Task ParseAsync(string[] args)
        {
            throw new NotImplementedException();
        }

        public IConfigurableParameter<IFluentArgsBuilder<TParam1>, TParam1> WithDescription(string description)
        {
            throw new NotImplementedException();
        }

        public IConfigurableParameter<IFluentArgsBuilder<TParam1>, TParam1> WithExamples(TParam1 example, params TParam1[] moreExamples)
        {
            throw new NotImplementedException();
        }

        public IConfigurableParameter<IFluentArgsBuilder<TParam1>, TParam1> WithExamples(string example, params string[] moreExamples)
        {
            throw new NotImplementedException();
        }

        public IConfigurableParameter<IFluentArgsBuilder<TParam1>, TParam1> WithParser(Func<string, TParam1> parser)
        {
            throw new NotImplementedException();
        }
    }
}
