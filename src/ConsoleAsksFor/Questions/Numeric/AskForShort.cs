using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAsksFor
{
    public static partial class AskForAppender
    {
        /// <summary>
        /// Ask for <see cref="short"/>.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="questionText"></param>
        /// <param name="range"></param>
        /// <param name="defaultValue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<short> AskForShort(
            this IConsole console,
            string questionText,
            RangeConstraint<short>? range = null,
            short? defaultValue = null,
            CancellationToken cancellationToken = default)
        {
            return (short)await console.AskForDecimal(
                questionText,
                Scale.Zero,
                ToDecimalRangeConstraint(range),
                defaultValue,
                cancellationToken);
        }

        private static RangeConstraint<decimal> ToDecimalRangeConstraint(RangeConstraint<short>? range)
            => RangeConstraint.Between<decimal>(
                range?.Min ?? short.MinValue,
                range?.Max ?? short.MaxValue);
    }
}