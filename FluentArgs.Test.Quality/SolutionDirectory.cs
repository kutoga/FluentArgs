namespace FluentArgs.Test.Quality
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal static class SolutionDirectory
    {
        public static IEnumerable<FileInfo> FindProjects()
        {
            var slnFile = FindSolutionFileOfCurrentProject(new DirectoryInfo(Environment.CurrentDirectory));

            return slnFile.Directory.EnumerateFiles("*.csproj", SearchOption.AllDirectories);
        }

        private static FileInfo FindSolutionFileOfCurrentProject(DirectoryInfo projectDirectory)
        {
            var solutionFileInCurrentDir = FindSolutionFileInDirectory(projectDirectory);
            if (!(solutionFileInCurrentDir is null))
            {
                return solutionFileInCurrentDir;
            }

            var parentDir = projectDirectory.Parent;
            if (parentDir is null)
            {
                throw new Exception("No sln file found.");
            }

            return FindSolutionFileOfCurrentProject(parentDir);
        }

        private static FileInfo? FindSolutionFileInDirectory(DirectoryInfo projectDirectory)
        {
            var slnFiles = projectDirectory
                .EnumerateFiles("*.sln", SearchOption.TopDirectoryOnly)
                .ToList();

            if (slnFiles.Count > 1)
            {
                throw new Exception($"Found more than 1 sln in the directory {projectDirectory}.");
            }

            return slnFiles.FirstOrDefault();
        }
    }
}
