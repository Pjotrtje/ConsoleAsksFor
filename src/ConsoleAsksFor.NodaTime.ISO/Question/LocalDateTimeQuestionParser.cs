namespace ConsoleAsksFor.NodaTime.ISO;

internal sealed class LocalDateTimeQuestionParser
{
    private readonly DateTimeZone? _dateTimeZone;
    private readonly LocalDateTimeFormat _format;

    public string DateTimeZoneDescription => _dateTimeZone?.Id ?? "Local";

    public ClusteredRange<LocalDateTime> Range { get; }

    public LocalDateTimeQuestionParser(
        LocalDateTimeFormat format,
        DateTimeZone? dateTimeZone,
        ClusteredRange<LocalDateTime> range)
    {
        _format = format;
        _dateTimeZone = dateTimeZone;
        Range = range;
    }

    public bool TryParse(string answerAsString, out IEnumerable<string> errors, out LocalDateTime answer)
    {
        if (!TryParseExact(answerAsString, out errors, out answer))
        {
            return false;
        }

        var x = answer;
        return Range.SubRanges.Any(r => r.Contains(x));
    }

    public bool TryParseExact(string answerAsString, out IEnumerable<string> errors, out LocalDateTime answer)
    {
        var parseResult = _format.Pattern.Parse(answerAsString.Trim());
        if (!parseResult.Success)
        {
            errors = Enumerable.Empty<string>();
            answer = default;
            return false;
        }

        if (_dateTimeZone is not null && _dateTimeZone?.MapLocal(parseResult.Value).Count == 0)
        {
            errors = new[] { "This DateTime never occurs due to summer/winter time." };
            answer = default;
            return false;
        }

        errors = Enumerable.Empty<string>();
        answer = parseResult.Value;
        return parseResult.Success;
    }
}