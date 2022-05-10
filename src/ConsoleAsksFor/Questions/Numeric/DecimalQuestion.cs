namespace ConsoleAsksFor;

internal sealed class DecimalQuestion : IQuestion<decimal>
{
    public string SubType => $"{_format.DigitsAfterDecimalPoint}p";

    public bool MustObfuscateAnswer => false;

    public IIntellisense Intellisense => new DecimalQuestionIntellisense(_parser, _format);

    public string Text { get; }

    public string PrefilledValue => _defaultValue is null
        ? ""
        : _format.FormatAnswer(_defaultValue.Value);

    private readonly DecimalQuestionParser _parser;
    private readonly DecimalFormat _format;
    private readonly string? _additionalHint;
    private readonly decimal? _defaultValue;

    public DecimalQuestion(
        string text,
        Scale scale,
        RangeConstraint<decimal> range,
        string? additionalHint,
        decimal? defaultValue)
    {
        var format = new DecimalFormat(scale);
        _parser = new DecimalQuestionParser(format, range);
        _additionalHint = additionalHint;
        _defaultValue = defaultValue;
        _format = format;
        Text = text;
    }

    public IEnumerable<string> GetHints()
    {
        if (_additionalHint is not null)
        {
            yield return _additionalHint;
        }
        yield return Hint.ForRange(_parser.Range, _format.FormatAnswer);
    }

    public bool TryParse(string answerAsString, out IEnumerable<string> errors, out decimal answer)
        => _parser.TryParse(answerAsString, out errors, out answer);

    public string FormatAnswer(decimal answer)
        => _format.FormatAnswer(answer);
}
