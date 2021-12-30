namespace ConsoleAsksFor;

internal sealed class PasswordQuestion : IQuestion<string>
{
    public string? SubType => null;

    public bool MustObfuscateAnswer => true;

    public IIntellisense Intellisense { get; } = new NoIntellisense();

    public string Text { get; }

    public string PrefilledValue => string.Empty;

    public PasswordQuestion(string text)
        => Text = text;

    public IEnumerable<string> GetHints() => Enumerable.Empty<string>();

    public bool TryParse(string answerAsString, out IEnumerable<string> errors, out string answer)
    {
        var isCorrect = answerAsString.Any();
        errors = isCorrect
            ? Enumerable.Empty<string>()
            : new[] { "Password missing." };

        answer = answerAsString;
        return isCorrect;
    }

    public string FormatAnswer(string answer)
        => answer;
}