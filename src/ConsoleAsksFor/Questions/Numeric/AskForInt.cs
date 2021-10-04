using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAsksFor
{
    public static partial class AskForAppender
    {
        /// <summary>
        /// Ask for <see cref="int"/>.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="questionText"></param>
        /// <param name="range"></param>
        /// <param name="defaultValue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<int> AskForInt(
            this IConsole console,
            string questionText,
            RangeConstraint<int>? range = null,
            int? defaultValue = null,
            CancellationToken cancellationToken = default)
        {
            return (int)await console.AskForDecimal(
                questionText,
                Scale.Zero,
                ToDecimalRangeConstraint(range),
                defaultValue,
                cancellationToken);
        }

        private static RangeConstraint<decimal> ToDecimalRangeConstraint(RangeConstraint<int>? range)
            => RangeConstraint.Between<decimal>(
                range?.Min ?? int.MinValue,
                range?.Max ?? int.MaxValue);
    }
}