namespace ConsoleAsksFor;

internal sealed class StringBasedValueObject<TValueType> : IQuestion<TValueType> where TValueType : notnull
{
    public string SubType => _hint ?? "[No hint]";

    public bool MustObfuscateAnswer => false;

    public IIntellisense Intellisense { get; } = new NoIntellisense();

    public string Text { get; }

    public string PrefilledValue => _defaultValue ?? string.Empty;

    private readonly TryParse<TValueType> _tryParse;
    private readonly Func<TValueType, string> _toString;
    private readonly string? _defaultValue;
    private readonly string? _hint;

    public StringBasedValueObject(
        string text,
        TryParse<TValueType> tryParse,
        Func<TValueType, string> toString,
        string? hint,
        string? defaultValue)
    {
        Text = text;
        _tryParse = tryParse;
        _toString = toString;
        _defaultValue = defaultValue;
        _hint = hint;
    }

    public IEnumerable<string> GetHints()
    {
        if (_hint is not null)
        {
            yield return _hint;
        }
    }

    public bool TryParse(string answerAsString, out IEnumerable<string> errors, [MaybeNullWhen(false)] out TValueType answer)
    {
        errors = [];
        return _tryParse(answerAsString, out answer);
    }

    public string FormatAnswer(TValueType answer)
        => _toString(answer);
}
