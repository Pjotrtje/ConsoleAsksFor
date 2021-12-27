using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAsksFor;

public static partial class AskForAppender
{
    /// <summary>
    /// Ask for <see cref="DirectoryInfo"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<DirectoryInfo> AskForDirectory(
        this IConsole console,
        string questionText,
        DirectoryInfo? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var fileSystemExistence = FileSystemExistence.NewOrExisting;
        var question = new DirectoryQuestion(
            questionText,
            fileSystemExistence,
            defaultValue);

        return await console.Ask(question, cancellationToken);
    }
}