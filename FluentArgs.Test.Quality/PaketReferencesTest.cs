namespace FluentArgs.Test.Quality
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using FluentAssertions;
    using Xunit;

    public static class PaketReferencesTest
    {
        private static readonly IEnumerable<string> Analyzers = new[]
        {
            "StyleCop.Analyzers",
            "Microsoft.CodeAnalysis.FxCopAnalyzers"
        };

        [Theory]
        [MemberData(nameof(AnalyzersPerProject))]
        public static void ContainsAnalyzer(string analyzer, DirectoryInfo projectDirectory)
        {
            var paketReferences = projectDirectory.GetFiles("paket.references", SearchOption.TopDirectoryOnly);

            paketReferences.Should().HaveCount(1);
            var references = ReadPaketReferences(paketReferences[0]);
            references.Should().Contain(analyzer);
        }

        [Theory]
        [MemberData(nameof(FindProjectDirectories))]
        public static void Exists(DirectoryInfo projectDirectory)
        {
            var paketReferences = projectDirectory.EnumerateFiles("paket.references", SearchOption.TopDirectoryOnly);

            paketReferences.Should().NotBeNullOrEmpty($"project ({projectDirectory}) should use paket.");
        }

        [Theory]
        [MemberData(nameof(FindProjectDirectories))]
        public static void HaveOneReferencesFileOnly(DirectoryInfo projectDirectory)
        {
            var paketReferences = projectDirectory.EnumerateFiles("paket.references", SearchOption.AllDirectories);

            paketReferences.Should().HaveCount(1, "projects should use Paket correctly");
        }

        private static IEnumerable<object[]> FindProjectDirectories()
        {
            return SolutionDirectory
                .FindProjects()
                .Select(p => new[] { p.Directory });
        }

        private static IEnumerable<object[]> AnalyzersPerProject()
        {
            return SolutionDirectory
                .FindProjects()
                .Select(p => p.Directory)
                .SelectMany(p => Analyzers.Select(a => new object[] { a, p }));
        }

        private static ISet<string> ReadPaketReferences(FileInfo paketReferencesFile)
        {
            return File.ReadAllLines(paketReferencesFile.FullName)
                .Select(line => line.Trim())
                .ToHashSet();
        }
    }
}