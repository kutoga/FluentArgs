using System;
using System.Linq;
using System.Threading.Tasks;
using FluentArgs.Description;
using FluentArgs.Execution;
using FluentArgs.Extensions;
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
                await InitialStep.Execute(state).ConfigureAwait(false);
            }
            catch (ArgumentMissingException ex)
            {
                if (ex.ArgumentName != null)
                {
                    await errorLineWriter
                        .WriteLine($"Required argument '{ex.ArgumentName.Names.StringifyAliases()}' not found!")
                        .ConfigureAwait(false);
                }

                await errorLineWriter.WriteLine($"Argument description: {ex.Description}").ConfigureAwait(false);
            }
            catch (ArgumentParsingException ex)
            {
                if (ex.ArgumentName != null)
                {
                    await errorLineWriter
                        .WriteLine($"Could not parse argument '{ex.ArgumentName.Names.StringifyAliases()}'!")
                        .ConfigureAwait(false);
                }

                await errorLineWriter.WriteLine($"Parsing error: {ex.Description}").ConfigureAwait(false);
            }
        }
    }
}