using System.Threading;
using System.Threading.Tasks;

using NodaTime;

namespace ConsoleAsksFor.NodaTime.ISO;

public static partial class AskForAppender
{
    /// <summary>
    /// Ask for <see cref="LocalDateTime"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="range"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<LocalDateTime> AskForLocalDateTime(
        this IConsole console,
        string questionText,
        RangeConstraint<LocalDateTime>? range = null,
        LocalDateTime? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        var question = new LocalDateTimeQuestion(
            questionText,
            LocalDateTimeFormat.DateTime,
            null,
            range ?? RangeConstraint.None,
            defaultValue);

        return await console.Ask(question, cancellationToken);
    }
}