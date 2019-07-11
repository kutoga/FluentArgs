namespace FluentArgs
{
    using System;
    using System.Linq;

    public interface IGivenCommandInitial<TArgsBuilder>
    {
        IGivenThen<TArgsBuilder, IGivenCommand<TArgsBuilder>> HasValue<TParam>(TParam[] values, Func<string, TParam>? parser = null);

        IGivenThen<TArgsBuilder, IGivenCommand<TArgsBuilder>> Matches<TParam>(Func<TParam, bool> predicate, Func<string, TParam>? parser = null);
    }

    public static class IGivenCommandInitialExtension
    {
        public static IGivenThen<TArgsBuilder, IGivenCommand<TArgsBuilder>> HasValue<TArgsBuilder, TParam>(this IGivenCommandInitial<TArgsBuilder> givenCommand, TParam value, Func<string, TParam>? parser = null)
        {
            return givenCommand.HasValue(new[] { value }, parser);
        }

        public static IGivenThen<TArgsBuilder, IGivenCommand<TArgsBuilder>> HasValue<TArgsBuilder>(this IGivenCommandInitial<TArgsBuilder> givenCommand, string value, Func<string, string>? parser = null)
        {
            return givenCommand.HasValue<TArgsBuilder, string>(value, parser);
        }

        public static IGivenThen<TArgsBuilder, IGivenCommand<TArgsBuilder>> HasValue<TArgsBuilder, TParam>(this IGivenCommandInitial<TArgsBuilder> givenCommand, TParam value, params TParam[] moreValues)
        {
            return givenCommand.HasValue(new[] { value }.Concat(moreValues).ToArray());
        }

        public static IGivenThen<TArgsBuilder, IGivenCommand<TArgsBuilder>> HasValue<TArgsBuilder>(this IGivenCommandInitial<TArgsBuilder> givenCommand, string value, params string[] moreValues)
        {
            return givenCommand.HasValue(new[] { value }.Concat(moreValues).ToArray());
        }

        public static IGivenThen<TArgsBuilder, IGivenCommand<TArgsBuilder>> Matches<TArgsBuilder>(this IGivenCommandInitial<TArgsBuilder> givenCommand, Func<string, bool> predicate, Func<string, string>? parser = null)
        {
            return givenCommand.Matches(predicate, parser);
        }
    }
}
