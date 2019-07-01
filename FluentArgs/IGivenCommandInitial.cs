namespace FluentArgs
{
    using System;

    public interface IGivenCommandInitial<TArgsBuilder>
    {
        IGivenThen<TArgsBuilder, IGivenCommand<TArgsBuilder>> HasValue<TParam>(TParam value, Func<string, TParam>? parser = null);

        IGivenThen<TArgsBuilder, IGivenCommand<TArgsBuilder>> Matches<TParam>(Func<TParam, bool> predicate, Func<string, TParam>? parser = null);
    }

    public static class IGivenCommandInitialExtension
    {
        public static IGivenThen<TArgsBuilder, IGivenCommand<TArgsBuilder>> HasValue<TArgsBuilder>(this IGivenCommandInitial<TArgsBuilder> givenCommand, string value, Func<string, string>? parser = null)
        {
            return givenCommand.HasValue(value, parser);
        }

        public static IGivenThen<TArgsBuilder, IGivenCommand<TArgsBuilder>> Matches<TArgsBuilder>(this IGivenCommandInitial<TArgsBuilder> givenCommand, Func<string, bool> predicate, Func<string, string>? parser = null)
        {
            return givenCommand.Matches(predicate, parser);
        }
    }
}
