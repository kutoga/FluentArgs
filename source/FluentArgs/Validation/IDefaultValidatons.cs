namespace FluentArgs.Validation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;

    public interface IDefaultValidatons
    {
        /* General */
        ValidationFunc<T> OnlyContain<T, TElement>(params TElement[] values)
            where T : IEnumerable<TElement>;

        ValidationFunc<T> OnlyContain<T, TElement>(IEnumerable<TElement> values)
            where T : IEnumerable<TElement>;

        ValidationFunc<T> BeOneOf<T>(params T[] values);

        ValidationFunc<T> BeOneOf<T>(IEnumerable<T> values);

        /* FileSystem */

        bool BeAnExistingFile(string file, out string? errorMessage);

        bool BeAnExistingFile(FileInfo file, out string? errorMessage);

        bool BeAnExistingDirectory(string directory, out string? errorMessage);

        bool BeAnExistingDirectory(DirectoryInfo directory, out string? errorMessage);

        /* Numerical */

        bool BePositive(int x, out string? errorMessage);

        bool BeNegative(int x, out string? errorMessage);

        ValidationFunc<int> BeInRange(int minInclusive, int maxExclusive);

        /* Text */

        bool BeAnEmail(string text, out string? errorMessage);

        ValidationFunc<string> OnlyContain(params char[] characters);

        ValidationFunc<string> Contain(string subString);

        bool BeUppercase(string text, out string? errorMessage);

        bool BeLowercase(string text, out string? errorMessage);

        ValidationFunc<string> Match(string regex);

        ValidationFunc<string> Match(Regex regex);

        /* Date */

        ValidationFunc<DateTimeOffset> BeBefore(DateTimeOffset reference);

        ValidationFunc<DateTimeOffset> BeAfter(DateTimeOffset reference);

        ValidationFunc<DateTimeOffset> BeBetween(DateTimeOffset beginInclusive, DateTimeOffset endExclusive);

        bool BeInThePast(DateTimeOffset date, out string? errorMessage);

        bool BeInTheFuture(DateTimeOffset date, out string? errorMessage);

        ValidationFunc<TimeSpan> BeShorterThan(TimeSpan reference);

        ValidationFunc<TimeSpan> BeLongerThan(TimeSpan reference);

        /* etc. */

        /* Call:...
         * 
         * .WithValidation(ItMust.Be.Positive)
         * 
         * or:
         * 
         * .WithValidation(ItMust.NotBe.InRange(1, 100))
         * 
         */
    }
}
