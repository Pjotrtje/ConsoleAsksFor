using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor;

internal sealed class DecimalQuestionParser
{
    private readonly DecimalFormat _format;
    private readonly NumberStyles _parseNumberStyles;

    public Range<decimal> Range { get; }

    public DecimalQuestionParser(
        DecimalFormat format,
        RangeConstraint<decimal> range)
    {
        _format = format;

        var allowedRange = _format.GetAllowedRange();
        var inputRange = GetInputRange(range, allowedRange, format);

        Range = allowedRange.HasOverlap(inputRange, out var overlappingRange)
            ? overlappingRange
            : throw InvalidRangeException.Create(inputRange, allowedRange);

        _parseNumberStyles = format.DigitsAfterDecimalPoint == 0
            ? NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands
            : NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint;
    }

    private static Range<decimal> GetInputRange(RangeConstraint<decimal> range, Range<decimal> allowedRange, DecimalFormat format)
    {
        var min = (range.Min ?? allowedRange.Min).TruncateMinValue(format.DigitsAfterDecimalPoint);
        var max = (range.Max ?? allowedRange.Max).TruncateMaxValue(format.DigitsAfterDecimalPoint);
        return new Range<decimal>(min, max);
    }

    public bool TryParse(string answerAsString, out IEnumerable<string> errors, out decimal answer)
    {
        errors = Enumerable.Empty<string>();
        return TryParseExact(answerAsString, out answer) && Range.Contains(answer);
    }

    public bool TryParseExact(string answerAsString, out decimal answer)
    {
        var canBeParsed = decimal.TryParse(answerAsString.Trim(), _parseNumberStyles, CultureInfo.InvariantCulture, out var d);
        if (!canBeParsed)
        {
            answer = default;
            return false;
        }

        var isCorrect = d.GetAmountOfDigitsAfterDecimalPoint() <= _format.DigitsAfterDecimalPoint;
        answer = isCorrect
            ? d
            : default;

        return isCorrect;
    }
}