namespace FluentArgs
{
    using FluentArgs.Builder;

    public static class FluentArgsBuilder
    {
        public static IInitialFluentArgsBuilder New()
        {
            return new StepBuilder();
        }

        public static IInitialFluentArgsBuilder NewWithDefaultConfigs()
        {
            return New().DefaultConfigs();
        }
    }
}
