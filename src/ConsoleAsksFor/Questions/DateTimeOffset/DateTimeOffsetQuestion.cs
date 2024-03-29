﻿namespace ConsoleAsksFor;

internal sealed class DateTimeOffsetQuestion : IQuestion<DateTimeOffset>
{
    public string SubType => $"{_parser.TimeZoneInfoDescription} - {_format.Pattern}";

    public bool MustObfuscateAnswer => false;

    public IIntellisense Intellisense => new DateTimeOffsetQuestionIntellisense(_parser, _format);

    public string Text { get; }

    public string PrefilledValue => _defaultValue is null
        ? ""
        : _format.FormatAnswer(_defaultValue.Value);

    private readonly DateTimeOffsetQuestionParser _parser;
    private readonly DateTimeOffsetFormat _format;
    private readonly DateTimeOffset? _defaultValue;

    public DateTimeOffsetQuestion(
        string text,
        DateTimeOffsetFormat format,
        TimeZoneInfo? timeZone,
        ClusteredRange<DateTimeOffset> range,
        DateTimeOffset? defaultValue)
    {
        Text = text;
        _parser = new DateTimeOffsetQuestionParser(format, timeZone, range);
        _format = format;
        _defaultValue = defaultValue;
    }

    public IEnumerable<string> GetHints()
    {
        yield return Hint.ForFormat($"'{_format.Pattern}' ({_parser.TimeZoneInfoDescription})");
        yield return Hint.ForRange(_parser.Range, _format.FormatAnswer);
    }

    public bool TryParse(string answerAsString, out IEnumerable<string> errors, out DateTimeOffset answer)
        => _parser.TryParse(answerAsString, out errors, out answer);

    public string FormatAnswer(DateTimeOffset answer)
        => _format.FormatAnswer(answer);
}
