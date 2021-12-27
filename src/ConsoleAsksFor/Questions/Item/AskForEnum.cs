namespace ConsoleAsksFor;

public static partial class AskForAppender
{
    /// <summary>
    /// Ask for <see cref="Enum"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<T> AskForEnum<T>(
        this IConsole console,
        string questionText,
        T? defaultValue = null,
        CancellationToken cancellationToken = default)
        where T : struct, Enum
    {
        return await console.AskForItem(
            questionText,
            Enum.GetValues<T>(),
            e => e.ToString(),
            defaultValue,
            cancellationToken);
    }
}