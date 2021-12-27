namespace ConsoleAsksFor.NodaTime.ISO;

internal sealed class LocalDateTimeQuestionParser
{
    private readonly DateTimeZone? _dateTimeZone;
    public string DateTimeZoneDescription => _dateTimeZone?.Id ?? "Local";
    private readonly LocalDateTimeFormat _format;

    public Range<LocalDateTime> Range { get; }

    public LocalDateTimeQuestionParser(
        LocalDateTimeFormat format,
        DateTimeZone? dateTimeZone,
        RangeConstraint<LocalDateTime> range)
    {
        _format = format;
        _dateTimeZone = dateTimeZone;
        Range = new(
            range.Min ?? LocalDate.MinIsoValue.At(new LocalTime(00, 00, 00)),
            range.Max ?? LocalDate.MaxIsoValue.At(new LocalTime(23, 59, 59)));
    }

    public bool TryParse(string answerAsString, out IEnumerable<string> errors, out LocalDateTime answer)
    {
        return TryParseExact(answerAsString, out errors, out answer) && Range.Contains(answer);
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