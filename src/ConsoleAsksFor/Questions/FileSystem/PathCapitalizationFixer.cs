using System.IO;

namespace ConsoleAsksFor
{
    internal static class PathCapitalizationFixer
    {
        public static string Fix(string path)
        {
            if (Directory.Exists(path))
            {
                return Fix(new DirectoryInfo(path));
            }

            if (File.Exists(path))
            {
                return Fix(new FileInfo(path));
            }

            return path;
        }

        public static string Fix(DirectoryInfo directory)
        {
            if (!directory.Exists)
            {
                return directory.FullName;
            }

            var parentDirInfo = directory.Parent;
            if (null == parentDirInfo)
            {
                return directory.Name;
            }
            return Path.Combine(
                Fix(parentDirInfo),
                parentDirInfo.GetDirectories(directory.Name)[0].Name);
        }

        public static string Fix(FileInfo file)
        {
            if (!file.Exists)
            {
                return file.FullName;
            }
            var dirInfo = file.Directory!;
            return Path.Combine(
                Fix(dirInfo),
                dirInfo.GetFiles(file.Name)[0].Name);
        }
    }
}