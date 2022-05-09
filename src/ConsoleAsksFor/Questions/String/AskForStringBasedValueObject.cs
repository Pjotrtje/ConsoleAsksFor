namespace ConsoleAsksFor;

public static partial class AskForAppender
{
    /// <summary>
    /// Ask for string based value object.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="tryParse"></param>
    /// <param name="hint"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="toString"></param>
    /// <returns></returns>
    public static async Task<TValueType> AskForStringBasedValueObject<TValueType>(
        this IConsole console,
        string questionText,
        TryParse<TValueType> tryParse,
        Func<TValueType, string> toString,
        string? hint,
        string? defaultValue = null,
        CancellationToken cancellationToken = default)
        where TValueType : notnull
    {
        var question = new StringBasedValueObject<TValueType>(
            questionText,
            tryParse,
            toString,
            hint,
            defaultValue);

        return await console.Ask(question, cancellationToken);
    }
}
