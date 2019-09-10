using System;
using System.Linq;
using System.Threading.Tasks;
using FluentArgs.Description;
using FluentArgs.Execution;
using FluentArgs.Help;

namespace FluentArgs
{
    internal class FluentArgsDefinition : IParsableFromState
    {
        private readonly Name? helpFlag;

        private readonly ILineWriter errorLineWriter;

        public FluentArgsDefinition(InitialStep initialStep, Name? helpFlag, ILineWriter errorLineWriter)
        {
            InitialStep = initialStep;
            this.helpFlag = helpFlag;
            this.errorLineWriter = errorLineWriter;
        }

        public InitialStep InitialStep { get; }

        public void Parse(params string[] args)
        {
            // TODO: unpack innerexception
            ParseAsync(args).Wait();
        }

        public Task ParseAsync(params string[] args)
        {
            return ParseFromState(State.InitialState(args));
        }

        public async Task ParseFromState(State state)
        {
            try
            {
                await InitialStep.Execute(state);
            }
            catch (ArgumentMissingException ex)
            {
                await errorLineWriter.WriteLines(
                    $"Required argument '{ex.ArgumentName.Names)}");
            }
            catch (Exception e)
            {
                /* todo: need error writer or something...; maybe the helpwriter only needs stdout? and the "error" writer needs stderr? */
                throw;
            }
        }
    }
}