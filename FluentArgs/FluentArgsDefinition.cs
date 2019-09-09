using System;
using System.Threading.Tasks;
using FluentArgs.Description;
using FluentArgs.Execution;

namespace FluentArgs
{
    internal class FluentArgsDefinition : IParsableFromState
    {
        public FluentArgsDefinition(InitialStep initialStep, Name? helpFlag)
        {
            InitialStep = initialStep;
            HelpFlag = helpFlag;
        }

        public InitialStep InitialStep { get; }

        public Name? HelpFlag { get; }

        public void Parse(params string[] args)
        {
            // TODO: unpack innerexception
            ParseAsync(args).Wait();
        }

        public Task ParseAsync(params string[] args)
        {
            return ParseFromState(State.InitialState(args));
        }

        public Task ParseFromState(State state)
        {
            try
            {
                return InitialStep.Execute(state);
            }
            catch (Exception e)
            {
                /* todo: need error writer or something...; maybe the helpwriter only needs stdout? and the "error" writer needs stderr? */
                throw;
            }
        }
    }
}