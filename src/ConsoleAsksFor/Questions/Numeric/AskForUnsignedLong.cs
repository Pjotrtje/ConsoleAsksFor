using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAsksFor;

public static partial class AskForAppender
{
    /// <summary>
    /// Ask for <see cref="ulong"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="range"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<ulong> AskForUnsignedLong(
        this IConsole console,
        string questionText,
        RangeConstraint<ulong>? range = null,
        ulong? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        return (ulong)await console.AskForDecimal(
            questionText,
            Scale.Zero,
            ToDecimalRangeConstraint(range),
            defaultValue,
            cancellationToken);
    }

    private static RangeConstraint<decimal> ToDecimalRangeConstraint(RangeConstraint<ulong>? range)
        => RangeConstraint.Between<decimal>(
            range?.Min ?? ulong.MinValue,
            range?.Max ?? ulong.MaxValue);
}