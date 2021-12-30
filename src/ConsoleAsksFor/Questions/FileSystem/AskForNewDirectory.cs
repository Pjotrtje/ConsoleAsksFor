namespace ConsoleAsksFor;

public static partial class AskForAppender
{
    /// <summary>
    /// Ask for new <see cref="DirectoryInfo"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<DirectoryInfo> AskForNewDirectory(
        this IConsole console,
        string questionText,
        DirectoryInfo? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var question = new DirectoryQuestion(
            questionText,
            FileSystemExistence.New,
            defaultValue);

        return await console.Ask(question, cancellationToken);
    }
}