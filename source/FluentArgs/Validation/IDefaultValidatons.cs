namespace FluentArgs.Validation
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;

    public interface IDefaultValidatons
    {
        /* General */
        IValidation<T> OnlyContain<T, TElement>(params TElement[] values)
            where T : IEnumerable<TElement>;

        IValidation<T> OnlyContain<T, TElement>(IEnumerable<TElement> values)
            where T : IEnumerable<TElement>;

        IValidation<T> BeOneOf<T>(params T[] values);

        IValidation<T> BeOneOf<T>(IEnumerable<T> values);

        /* FileSystem */

        bool BeAnExistingFile(string file, out string? errorMessage);

        bool BeAnExistingFile(FileInfo file, out string? errorMessage);

        bool BeAnExistingDirectory(string directory, out string? errorMessage);

        bool BeAnExistingDirectory(DirectoryInfo directory, out string? errorMessage);

        /* Numerical */

        bool BePositive(int x, out string? errorMessage);

        bool BeNegative(int x, out string? errorMessage);

        IValidation<int> BeInRange(int minInclusive, int maxExclusive);

        /* Text */

        bool BeAnEmail(string text, out string? errorMessage);

        IValidation<string> OnlyContain(params char[] characters);

        IValidation<string> Contain(string subString);

        bool BeUppercase(string text, out string? errorMessage);

        bool BeLowercase(string text, out string? errorMessage);

        IValidation<string> Match(string regex);

        IValidation<string> Match(Regex regex);

        /* Date */

        IValidation<DateTimeOffset> BeBefore(DateTimeOffset reference);

        IValidation<DateTimeOffset> BeAfter(DateTimeOffset reference);

        IValidation<DateTimeOffset> BeBetween(DateTimeOffset beginInclusive, DateTimeOffset endExclusive);

        bool BeInThePast(DateTimeOffset date, out string? errorMessage);

        bool BeInTheFuture(DateTimeOffset date, out string? errorMessage);

        IValidation<TimeSpan> BeShorterThan(TimeSpan reference);

        IValidation<TimeSpan> BeLongerThan(TimeSpan reference);

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
