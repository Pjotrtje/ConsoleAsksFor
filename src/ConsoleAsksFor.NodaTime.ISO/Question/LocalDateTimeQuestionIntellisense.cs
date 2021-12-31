namespace ConsoleAsksFor.NodaTime.ISO;

internal sealed class LocalDateTimeQuestionIntellisense : IIntellisense
{
    private readonly LocalDateTimeQuestionParser _parser;
    private readonly LocalDateTimeFormat _format;

    public LocalDateTimeQuestionIntellisense(
        LocalDateTimeQuestionParser parser,
        LocalDateTimeFormat format)
    {
        _parser = parser;
        _format = format;
    }

    public string? CompleteValue(string value)
        => Handle(value, value, IntellisenseDirection.None);

    public string? PreviousValue(string value, string hint)
        => Handle(value, hint, IntellisenseDirection.Previous);

    public string? NextValue(string value, string hint)
        => Handle(value, hint, IntellisenseDirection.Next);

    private string? Handle(string value, string hint, IntellisenseDirection direction)
    {
        var trimmedHint = hint.Trim();
        if (!_parser.TryParse(value, out _, out var answer))
        {
            return TryCompleteValue(trimmedHint, direction);
        }

        var overlap = GetRangesOverlappingWithIntellisense(trimmedHint);
        if (overlap is null)
        {
            return null;
        }

        var newAnswer = SafeMove(answer, overlap, direction);
        return _format.FormatAnswer(newAnswer);
    }

    private string? TryCompleteValue(string value, IntellisenseDirection direction)
    {
        var overlap = GetRangesOverlappingWithIntellisense(value);
        if (overlap is null)
        {
            return null;
        }

        return direction == IntellisenseDirection.Previous
            ? _format.FormatAnswer(overlap.Max())
            : _format.FormatAnswer(overlap.Min());
    }

    private ClusteredRange<LocalDateTime>? GetRangesOverlappingWithIntellisense(string value)
    {
        var minValue = _format.IntellisenseMinPatterns
            .Select(p => TryCompleteByTemplate(value, p))
            .Min(d => d);

        var maxValue = _format.IntellisenseMaxPatterns
            .Select(p => TryCompleteByTemplate(value, p))
            .Max(d => d);

        if (!minValue.HasValue || !maxValue.HasValue)
        {
            return null;
        }

        var intellisenseRange = new Range<LocalDateTime>(minValue.Value, maxValue.Value);

        return _parser.Range.HasOverlap(intellisenseRange, out var overlap)
            ? overlap
            : null;
    }

    private LocalDateTime? TryCompleteByTemplate(string value, string template)
    {
        if (template.Length <= value.Length)
        {
            return null;
        }

        var answerAsString = $"{value}{template[value.Length..]}";
        return _parser.TryParseExact(answerAsString, out _, out var answer)
            ? answer
            : null;
    }

    private LocalDateTime SafeMove(LocalDateTime answer, ClusteredRange<LocalDateTime> overlap, IntellisenseDirection direction)
    {
        try
        {
            var delta = GetDelta(direction);
            var newAnswer = answer.Plus(delta);
            if (overlap.Contains(newAnswer))
            {
                return newAnswer;
            }

            if (overlap.SubRanges.First().Contains(answer))
            {
                return direction == IntellisenseDirection.Previous
                    ? overlap.SubRanges.Last().Max
                    : overlap.SubRanges.Last().Min;
            }

            if (overlap.SubRanges.Last().Contains(answer))
            {
                return direction == IntellisenseDirection.Previous
                    ? overlap.SubRanges.First().Max
                    : overlap.SubRanges.First().Min;
            }
        }
        catch (OverflowException)
        {
        }

        return direction == IntellisenseDirection.Previous
            ? overlap.Max()
            : overlap.Min();
    }

    private Period GetDelta(IntellisenseDirection direction)
        => direction switch
        {
            IntellisenseDirection.Previous => Period.Zero - _format.SmallestIncrement,
            IntellisenseDirection.Next => _format.SmallestIncrement,
            _ => Period.Zero,
        };
}