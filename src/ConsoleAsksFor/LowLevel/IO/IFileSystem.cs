using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleAsksFor;

internal interface IFileSystem
{
    bool FileExists(string path);

    Task<string> FileReadAllTextAsync(string path);

    Task FileWriteAllTextAsync(string path, string contents);

    Task FileAppendAllLinesAsync(string path, IEnumerable<string> contents);

    void CreateDirectory(string path);
}