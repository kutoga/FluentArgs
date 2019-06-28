namespace FluentArgs
{
    using FluentArgs.Builder;

    public static class FluentArgsBuilder
    {
        public static IFluentArgsBuilder New()
        {
            return new StepBuilder();
        }
    }
}
