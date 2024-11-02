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
        TValueType? defaultValue = null,
        CancellationToken cancellationToken = default)
        where TValueType : class
    {
        var question = new StringBasedValueObjectQuestion<TValueType>(
            questionText,
            tryParse,
            toString,
            hint,
            defaultValue is null ? null : toString(defaultValue));

        return await console.Ask(question, cancellationToken);
    }

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
        TValueType? defaultValue = null,
        CancellationToken cancellationToken = default)
        where TValueType : struct
    {
        var question = new StringBasedValueObjectQuestion<TValueType>(
            questionText,
            tryParse,
            toString,
            hint,
            defaultValue is null ? null : toString(defaultValue.Value));

        return await console.Ask(question, cancellationToken);
    }

    /// <summary>
    /// Ask for string based value object.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="hint"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<TValueType> AskForStringBasedValueObject<TValueType>(
        this IConsole console,
        string questionText,
        string? hint,
        TValueType? defaultValue = default,
        CancellationToken cancellationToken = default)
        where TValueType : class, IParsable<TValueType>, IFormattable
    {
        static bool TransformedTryParse(string str, [MaybeNullWhen(false)] out TValueType result)
            => TValueType.TryParse(str, null!, out result);

        var question = new StringBasedValueObjectQuestion<TValueType>(
            questionText,
            TransformedTryParse,
            x => x.ToString(null, null),
            hint,
            defaultValue?.ToString(null, null));

        return await console.Ask(question, cancellationToken);
    }

    /// <summary>
    /// Ask for string based value object.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="hint"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<TValueType> AskForStringBasedValueObject<TValueType>(
        this IConsole console,
        string questionText,
        string? hint,
        TValueType? defaultValue = null,
        CancellationToken cancellationToken = default)
        where TValueType : struct, IParsable<TValueType>, IFormattable
    {
        static bool TransformedTryParse(string str, [MaybeNullWhen(false)] out TValueType result)
            => TValueType.TryParse(str, null!, out result);

        var question = new StringBasedValueObjectQuestion<TValueType>(
            questionText,
            TransformedTryParse,
            x => x.ToString(null, null),
            hint,
            defaultValue?.ToString(null, null));

        return await console.Ask(question, cancellationToken);
    }
}
