namespace FluentArgs.Test.Helpers
{
    using System.Collections;
    using System.Collections.Generic;
    using FluentAssertions;
    using FluentAssertions.Collections;

    public static class FluentAssertionsExtensions
    {
        public static AndConstraint<TAssertions> BeEquivalentWithSameOrdering<TSubject, TAssertions, TExpectation>(
            this CollectionAssertions<TSubject, TAssertions> assertions, IEnumerable<TExpectation> expectation)
            where TSubject : IEnumerable //TODO: überall multi "where"-statements wie hier machen
            where TAssertions : CollectionAssertions<TSubject, TAssertions>
        {
            return assertions.BeEquivalentTo(expectation, options => options.WithStrictOrdering());
        }

        public static AndConstraint<TAssertions> BeEquivalentWithSameOrdering<TSubject, TAssertions>(
            this CollectionAssertions<TSubject, TAssertions> assertions, params object[] expectation)
            where TSubject : IEnumerable //TODO: überall multi "where"-statements wie hier machen
            where TAssertions : CollectionAssertions<TSubject, TAssertions>
        {
            return assertions.BeEquivalentTo(expectation, options => options.WithStrictOrdering());
        }

        public static AndConstraint<StringCollectionAssertions> BeEquivalentWithSameOrdering(
            this StringCollectionAssertions assertions, IEnumerable<string> expectation)
        {
            return assertions.BeEquivalentTo(expectation, options => options.WithStrictOrdering());
        }

        public static AndConstraint<StringCollectionAssertions> BeEquivalentWithSameOrdering(
            this StringCollectionAssertions assertions, params string[] expectation)
        {
            return assertions.BeEquivalentTo(expectation, options => options.WithStrictOrdering());
        }
    }
}
