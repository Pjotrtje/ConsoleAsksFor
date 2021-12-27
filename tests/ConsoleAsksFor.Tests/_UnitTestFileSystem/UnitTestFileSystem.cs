using System.IO;
using System.Linq;
using System.Reflection;

namespace ConsoleAsksFor.Tests;

public static class UnitTestFileSystem
{
    private static string Combine(params string[] relativeParts)
    {
        var unitTestFileSystem = GetUnitTestFileSystemDirectory();
        var parts = relativeParts.Prepend(unitTestFileSystem).ToArray();
        return Path.Combine(parts);
    }

    private static string GetUnitTestFileSystemDirectory()
    {
        var unitTestFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        return Path.Combine(unitTestFolder, "_UnitTestFileSystem");
    }

    public static string Location => GetUnitTestFileSystemDirectory();

    public static class Drive
    {
        public static string Location => Path.GetPathRoot(GetUnitTestFileSystemDirectory())!;
    }

    public static class ExistingDirectory1
    {
        public static string Location => Combine(nameof(ExistingDirectory1));

        public static class ExistingFile1a
        {
            public static string Location => Combine(ExistingDirectory1.Location, $"{nameof(ExistingFile1a)}.txt");
        }

        public static class ExistingFile1b
        {
            public static string Location => Combine(ExistingDirectory1.Location, $"{nameof(ExistingFile1b)}.txt");
        }

        public static class NotExistingFile
        {
            public static string Location => Combine(ExistingDirectory1.Location, $"{nameof(NotExistingFile)}.txt");
        }

        public static class NotExistingFileWithoutExtension
        {
            public static string Location => Combine(ExistingDirectory1.Location, nameof(NotExistingFileWithoutExtension));
        }

        public static class NotExistingFileWithInvalidChars
        {
            public static string Location => Combine(ExistingDirectory1.Location, $"|{nameof(NotExistingFileWithInvalidChars)}.txt");
        }
    }

    public static class ExistingDirectory2
    {
        public static string Location => Combine(nameof(ExistingDirectory2));
    }

    public static class NotExistingDirectory
    {
        public static string Location => Combine(nameof(NotExistingDirectory));
    }

    public static class NotExistingDirectoryWithInvalidChars
    {
        public static string Location => Combine('|'.ToString());

        public static class NotExistingFile
        {
            public static string Location => Combine(NotExistingDirectoryWithInvalidChars.Location, $"{nameof(NotExistingFile)}.txt");
        }
    }
}