using System.Threading;
using System.Threading.Tasks;

using NodaTime;

namespace ConsoleAsksFor.NodaTime.ISO;

public static partial class AskForAppender
{
    /// <summary>
    /// Ask for <see cref="LocalTime"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="range"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<LocalTime> AskForLocalTime(
        this IConsole console,
        string questionText,
        RangeConstraint<LocalTime>? range = null,
        LocalTime? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var question = new LocalDateTimeQuestion(
            questionText,
            LocalDateTimeFormat.Time,
            null,
            ToLocalDateTimeConstraint(range),
            defaultValue?.ToLocalDateTime());

        var localDateTime = await console.Ask(question, cancellationToken);
        return new LocalTime(localDateTime.Hour, localDateTime.Minute, localDateTime.Second);
    }

    private static RangeConstraint<LocalDateTime> ToLocalDateTimeConstraint(RangeConstraint<LocalTime>? rangeConstraint)
        => RangeConstraint.Between(
            (rangeConstraint?.Min ?? LocalTime.Midnight).ToLocalDateTime(),
            (rangeConstraint?.Max ?? LocalTime.Midnight.PlusSeconds(-1)).ToLocalDateTime());

    private static LocalDateTime ToLocalDateTime(this LocalTime localTime)
        => new LocalDate(2000, 1, 1).At(localTime);
}