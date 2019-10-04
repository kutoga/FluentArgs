namespace FluentArgs.Test.Quality
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.XPath;
    using FluentAssertions;
    using Xunit;

    public static class ProjectFilesTest
    {
        [Theory]
        [MemberData(nameof(AllProjects))]
        public static void ContainsAtMostOneNoWarnNode(FileInfo project)
        {
            var noWarnNode = GetSingleOrDefaultXmlNode(project, "//NoWarn");

            noWarnNode.Should().NotBeNull("project should contain exactly one NoWarn node.");
        }

        [Theory]
        [MemberData(nameof(AllProjects))]
        public static void ContainsPaketTarget(FileInfo project)
        {
            var importNode = GetSingleOrDefaultXmlNode(project, "Project/Import[@Project='..\\.paket\\Paket.Restore.targets']");

            importNode.Should().NotBeNull("project should use Paket.");
        }

        [Theory]
        [MemberData(nameof(AllProjects))]
        public static void ContainsRuleset(FileInfo project)
        {
            var codeAnalysisRuleSetNode = GetSingleOrDefaultXmlNode(project, "Project/PropertyGroup/CodeAnalysisRuleSet[text()='..\\code.ruleset']");

            codeAnalysisRuleSetNode.Should().NotBeNull("project should use the given ruleset.");
        }

        [Theory]
        [MemberData(nameof(AllProjects))]
        public static void DoesNotContainPackageReferences(FileInfo project)
        {
            var packageReferenceNode = GetSingleOrDefaultXmlNode(project, "//PackageReference");
            var referenceNode = GetSingleOrDefaultXmlNode(project, "//Reference");

            packageReferenceNode.Should().BeNull("projects should not use NuGet.");
            referenceNode.Should().BeNull("projects should not use NuGet.");
        }

        [Theory]
        [MemberData(nameof(AllProjects))]
        public static void IgnoresNuGetDependencyConstraintErrors(FileInfo project)
        {
            const string nu1608 = "NU1608";
            var nu1608NoWarnNode = GetSingleOrDefaultXmlNode(project, $"Project/PropertyGroup[not(@*)][contains(NoWarn, '{nu1608}')]");

            nu1608NoWarnNode.Should().NotBeNull($"projects should ignore {nu1608} in all configurations.");
        }

        [Theory]
        [MemberData(nameof(AllProjects))]
        public static void TargetsAllowedFrameworkVersions(FileInfo project)
        {
            var targetFrameworkVersion = new[]
            {
                "netstandard1.0",
                "netstandard2.0",
                "netcoreapp2.0",
                "netcoreapp2.1",
                "netcoreapp2.2"
            };

            var codeAnalysisRuleSetNode = GetSingleOrDefaultXmlNode(project, $"Project/PropertyGroup/TargetFramework");
            codeAnalysisRuleSetNode?.InnerXml.Should().BeOneOf(targetFrameworkVersion);
        }

        [Theory]
        [MemberData(nameof(AllProjects))]
        public static void TreatsWarningsAsErrorsInRelease(FileInfo project)
        {
            const string releaseAnyCpu = "'$(Configuration)|$(Platform)'=='Release|AnyCPU'";
            var releasePropertyGroupNode = GetSingleOrDefaultXmlNode(project, $"Project/PropertyGroup[@Condition=\"{releaseAnyCpu}\"]");

            var treatWarningsAsErrors = releasePropertyGroupNode?.SelectSingleNode("//TreatWarningsAsErrors")?.ValueAsBoolean ?? false;
            var all = releasePropertyGroupNode?.SelectSingleNode("//WarningsAsErrors")?.IsEmptyElement ?? false;

            (all && treatWarningsAsErrors).Should().BeTrue("projects should treat all warnings as errors in release builds.");
        }

        [Theory]
        [MemberData(nameof(AllProjects))]
        public static void UsesAtLeastCS8Features(FileInfo project)
        {
            var languageVersionNode = GetSingleOrDefaultXmlNode(project, "Project/PropertyGroup[not(@*)]/LangVersion");
            var languageVersion = double.Parse(languageVersionNode?.InnerXml, CultureInfo.InvariantCulture);

            languageVersion.Should().BeGreaterOrEqualTo(8.0, "projects should use the C# 8.0 language version in all configurations.");
        }

        [Theory]
        [MemberData(nameof(AllProjects))]
        public static void ClassTypesShouldNotBeNullable(FileInfo project)
        {
            var nullableContextOptions = GetSingleOrDefaultXmlNode(project, "Project/PropertyGroup[not(@*)]/NullableContextOptions[text()='enable']");
            var nullableOptions = GetSingleOrDefaultXmlNode(project, "Project/PropertyGroup[not(@*)]/Nullable[text()='enable']");

            nullableContextOptions.Should().NotBeNull("projects should be configured in a way that in the deafualt behaviour all types are not nullable.");
            nullableOptions.Should().NotBeNull("projects should be configured in a way that in the deafualt behaviour all types are not nullable.");
        }

        private static IEnumerable<object[]> AllProjects()
        {
            return SolutionDirectory.FindProjects().Select(_ => new[] { _ });
        }

        private static XPathNavigator? GetSingleOrDefaultXmlNode(FileSystemInfo filename, string xPath)
        {
            XPathDocument proj;
            using (var reader = new XmlTextReader(filename.FullName) { DtdProcessing = DtdProcessing.Prohibit })
            {
                proj = new XPathDocument(reader);
            }

            var navigator = proj.CreateNavigator().Select(xPath);

            if (!navigator.MoveNext())
            {
                return null;
            }

            var result = navigator.Current;
            var hasMore = navigator.MoveNext();
            hasMore.Should().BeFalse($"{xPath} should occur at most once.");
            return result;
        }
    }
}
