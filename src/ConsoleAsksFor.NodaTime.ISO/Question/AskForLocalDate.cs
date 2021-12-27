using System.Threading;
using System.Threading.Tasks;

using NodaTime;

namespace ConsoleAsksFor.NodaTime.ISO;

public static partial class AskForAppender
{
    /// <summary>
    /// Ask for <see cref="LocalDate"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="range"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<LocalDate> AskForLocalDate(
        this IConsole console,
        string questionText,
        RangeConstraint<LocalDate>? range = null,
        LocalDate? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var question = new LocalDateTimeQuestion(
            questionText,
            LocalDateTimeFormat.Date,
            null,
            ToLocalDateTimeConstraint(range),
            defaultValue?.ToLocalDateTime());

        var result = await console.Ask(question, cancellationToken);
        return result.Date;
    }

    private static RangeConstraint<LocalDateTime> ToLocalDateTimeConstraint(RangeConstraint<LocalDate>? range)
        => new(
            range?.Min?.ToLocalDateTime(),
            range?.Max?.ToLocalDateTime());

    private static LocalDateTime ToLocalDateTime(this LocalDate localDate)
        => localDate + LocalTime.Midnight;
}