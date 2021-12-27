using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleAsksFor;

internal sealed class FileSystem : IFileSystem
{
    public bool FileExists(string path)
        => File.Exists(path);

    public async Task<string> FileReadAllTextAsync(string path)
        => await File.ReadAllTextAsync(path);

    public void CreateDirectory(string path)
        => Directory.CreateDirectory(path);

    public async Task FileWriteAllTextAsync(string path, string contents)
        => await File.WriteAllTextAsync(path, contents);

    public async Task FileAppendAllLinesAsync(string path, IEnumerable<string> contents)
        => await File.AppendAllLinesAsync(path, contents);
}