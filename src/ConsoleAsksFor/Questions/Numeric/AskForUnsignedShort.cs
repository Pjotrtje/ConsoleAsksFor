using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAsksFor;

public static partial class AskForAppender
{
    /// <summary>
    /// Ask for <see cref="ushort"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="range"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<ushort> AskForUnsignedShort(
        this IConsole console,
        string questionText,
        RangeConstraint<ushort>? range = null,
        ushort? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        return (ushort)await console.AskForDecimal(
            questionText,
            Scale.Zero,
            ToDecimalRangeConstraint(range),
            defaultValue,
            cancellationToken);
    }

    private static RangeConstraint<decimal> ToDecimalRangeConstraint(RangeConstraint<ushort>? range)
        => RangeConstraint.Between<decimal>(
            range?.Min ?? ushort.MinValue,
            range?.Max ?? ushort.MaxValue);
}