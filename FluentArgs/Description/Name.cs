using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentArgs.Description
{
    internal class Name
    {
        private Name(string[] names)
        {
            Names = names;
        }

        public string[] Names { get; }

        public static Name ValidateAndBuild(string name, params string[] moreNames)
        {
            return ValidateAndBuild(moreNames.Concat(new[] {name}).ToArray());
        }

        public static Name ValidateAndBuild(IReadOnlyCollection<string> names)
        {
            var duplicateNames = names
                .GroupBy(n => n)
                .ToDictionary(g => g.Key, g => g.Count())
                .Where(g => g.Value > 1)
                .Select(g => g.Key)
                .OrderBy(n => n)
                .ToArray();
            if (duplicateNames.Any())
            {
                throw new Exception($"Name aliases must be unique. The following names are used multiple times in the same definition: {string.Join(", ", duplicateNames)}");
            }

            if (names.Any(n => string.IsNullOrWhiteSpace(n)))
            {
                throw new Exception("A name must not only contain whitespace!");
            }

            return new Name(names
                .Distinct()
                .OrderBy(n => n)
                .ToArray());
        }
    }
}