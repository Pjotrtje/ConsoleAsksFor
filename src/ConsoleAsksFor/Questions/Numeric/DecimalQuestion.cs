using System.Collections.Generic;

using ConsoleAsksFor.Sdk;

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
    private readonly decimal? _defaultValue;

    public DecimalQuestion(
        string text,
        Scale scale,
        RangeConstraint<decimal> range,
        decimal? defaultValue)
    {
        var format = new DecimalFormat(scale);
        _parser = new DecimalQuestionParser(format, range);
        _defaultValue = defaultValue;
        _format = format;
        Text = text;
    }

    public IEnumerable<string> GetHints()
    {
        yield return Hint.ForRange(_parser.Range, _format.FormatAnswer);
    }

    public bool TryParse(string answerAsString, out IEnumerable<string> errors, out decimal answer)
        => _parser.TryParse(answerAsString, out errors, out answer);

    public string FormatAnswer(decimal answer)
        => _format.FormatAnswer(answer);
}