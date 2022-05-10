namespace ConsoleAsksFor;

public static partial class AskForAppender
{
    /// <summary>
    /// Ask for existing <see cref="FileInfo"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="allowedExtensions"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<FileInfo> AskForExistingFileName(
        this IConsole console,
        string questionText,
        IEnumerable<string>? allowedExtensions = null,
        FileInfo? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var question = new FileNameQuestion(
            questionText,
            FileSystemExistence.Existing,
            allowedExtensions,
            defaultValue);

        return await console.Ask(question, cancellationToken);
    }

    /// <summary>
    /// Ask for existing <see cref="FileInfo"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="allowedExtension"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<FileInfo> AskForExistingFileName(
        this IConsole console,
        string questionText,
        string allowedExtension,
        FileInfo? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var question = new FileNameQuestion(
            questionText,
            FileSystemExistence.Existing,
            new[] { allowedExtension },
            defaultValue);

        return await console.Ask(question, cancellationToken);
    }
}
