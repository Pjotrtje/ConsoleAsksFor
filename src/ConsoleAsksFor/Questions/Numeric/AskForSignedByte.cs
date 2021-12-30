namespace ConsoleAsksFor;

public static partial class AskForAppender
{
    /// <summary>
    /// Ask for <see cref="sbyte"/>.
    /// </summary>
    /// <param name="console"></param>
    /// <param name="questionText"></param>
    /// <param name="range"></param>
    /// <param name="defaultValue"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<sbyte> AskForSignedByte(
        this IConsole console,
        string questionText,
        RangeConstraint<sbyte>? range = null,
        sbyte? defaultValue = null,
        CancellationToken cancellationToken = default)
    {
        return (sbyte)await console.AskForDecimal(
            questionText,
            Scale.Zero,
            ToDecimalRangeConstraint(range),
            defaultValue,
            cancellationToken);
    }

    private static RangeConstraint<decimal> ToDecimalRangeConstraint(RangeConstraint<sbyte>? range)
        => RangeConstraint.Between<decimal>(
            range?.Min ?? sbyte.MinValue,
            range?.Max ?? sbyte.MaxValue);
}