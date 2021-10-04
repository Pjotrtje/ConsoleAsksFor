using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAsksFor
{
    public static partial class AskForAppender
    {
        /// <summary>
        /// Ask for <see cref="uint"/>.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="questionText"></param>
        /// <param name="range"></param>
        /// <param name="defaultValue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<uint> AskForUnsignedInt(
            this IConsole console,
            string questionText,
            RangeConstraint<uint>? range = null,
            uint? defaultValue = null,
            CancellationToken cancellationToken = default)
        {
            return (uint)await console.AskForDecimal(
                questionText,
                Scale.Zero,
                ToDecimalRangeConstraint(range),
                defaultValue,
                cancellationToken);
        }

        private static RangeConstraint<decimal> ToDecimalRangeConstraint(RangeConstraint<uint>? range)
            => RangeConstraint.Between<decimal>(
                range?.Min ?? uint.MinValue,
                range?.Max ?? uint.MaxValue);
    }
}