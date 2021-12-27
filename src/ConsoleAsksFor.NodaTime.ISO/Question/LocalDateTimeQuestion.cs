using System.Collections.Generic;

using ConsoleAsksFor.Sdk;

using NodaTime;

namespace ConsoleAsksFor.NodaTime.ISO;

internal sealed class LocalDateTimeQuestion : IQuestion<LocalDateTime>
{
    public string SubType => $"{_parser.DateTimeZoneDescription} - {_format.Pattern.PatternText}";

    public bool MustObfuscateAnswer => false;

    public IIntellisense Intellisense => new LocalDateTimeQuestionIntellisense(_parser, _format);

    public string Text { get; }

    public string PrefilledValue => _defaultValue is null
        ? ""
        : _format.FormatAnswer(_defaultValue.Value);

    private readonly LocalDateTimeQuestionParser _parser;
    private readonly LocalDateTimeFormat _format;
    private readonly LocalDateTime? _defaultValue;

    public LocalDateTimeQuestion(
        string text,
        LocalDateTimeFormat format,
        DateTimeZone? dateTimeZone,
        RangeConstraint<LocalDateTime> range,
        LocalDateTime? defaultValue)
    {
        Text = text;
        _parser = new LocalDateTimeQuestionParser(format, dateTimeZone, range);
        _format = format;
        _defaultValue = defaultValue;
    }

    public IEnumerable<string> GetHints()
    {
        yield return Hint.ForRange(_parser.Range, _format.FormatAnswer);
        yield return Hint.ForFormat($"'{_format.Pattern.PatternText}' ({_parser.DateTimeZoneDescription})");
    }

    public bool TryParse(string answerAsString, out IEnumerable<string> errors, out LocalDateTime answer)
        => _parser.TryParse(answerAsString, out errors, out answer);

    public string FormatAnswer(LocalDateTime answer)
        => _format.FormatAnswer(answer);
}