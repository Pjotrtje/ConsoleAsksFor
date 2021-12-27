using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAsksFor;

public static partial class AskForAppender
{
    /// <summary>
    /// Ask for <see cref="long"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="range"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<long> AskForLong(
        this IConsole console,
        string questionText,
        RangeConstraint<long>? range = null,
        long? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        return (long)await console.AskForDecimal(
            questionText,
            Scale.Zero,
            ToDecimalRangeConstraint(range),
            defaultValue,
            cancellationToken);
    }

    private static RangeConstraint<decimal> ToDecimalRangeConstraint(RangeConstraint<long>? range)
        => RangeConstraint.Between<decimal>(
            range?.Min ?? long.MinValue,
            range?.Max ?? long.MaxValue);
}