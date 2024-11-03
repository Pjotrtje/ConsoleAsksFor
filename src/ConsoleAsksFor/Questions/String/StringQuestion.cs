namespace ConsoleAsksFor;

internal sealed class StringQuestion : IQuestion<string>
{
    public string SubType => $"{_regex?.ToString() ?? "[No Regex]"}";

    public bool MustObfuscateAnswer => false;

    public IIntellisense Intellisense { get; } = new NoIntellisense();

    public string Text { get; }

    public string PrefilledValue => _defaultValue ?? string.Empty;

    private readonly Regex? _regex;
    private readonly string? _defaultValue;
    private readonly string? _hint;

    public StringQuestion(
        string text,
        (Regex Regex, string? Hint)? guidedRegex,
        string? defaultValue)
    {
        Text = text;
        _regex = guidedRegex?.Regex;
        _defaultValue = defaultValue;
        _hint = guidedRegex is not null
            ? guidedRegex.Value.Hint ?? $"With regex pattern: '{guidedRegex.Value.Regex}'."
            : null;
    }

    public IEnumerable<string> GetHints()
    {
        if (_hint is not null)
        {
            yield return _hint;
        }
    }

    public bool TryParse(string answerAsString, out IEnumerable<string> errors, out string answer)
    {
        errors = [];
        answer = answerAsString;
        return _regex?.IsMatch(answer) ?? true;
    }

    public string FormatAnswer(string answer)
        => answer;
}