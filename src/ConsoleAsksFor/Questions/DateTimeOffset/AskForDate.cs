using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAsksFor
{
    public static partial class AskForAppender
    {
        /// <summary>
        /// Ask for <see cref="DateTime"/> without time part.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="questionText"></param>
        /// <param name="kind">When <see cref="DateTimeKind.Unspecified"/> then <see cref="DateTimeKind.Local"/> is assumed.</param>
        /// <param name="range"></param>
        /// <param name="defaultValue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<DateTime> AskForDate(
            this IConsole console,
            string questionText,
            DateTimeKind kind = DateTimeKind.Local,
            RangeConstraint<DateTime>? range = null,
            DateTime? defaultValue = null,
            CancellationToken cancellationToken = default)
        {
            var timeZoneInfo = kind == DateTimeKind.Utc
                ? TimeZoneInfo.Utc
                : TimeZoneInfo.Local;

            var question = new DateTimeOffsetQuestion(
                questionText,
                DateTimeOffsetFormat.Date,
                timeZoneInfo,
                ToToDateTimeOffsetRangeConstraint(range, kind),
                defaultValue?.ToKind(kind));

            var result = await console.Ask(question, cancellationToken);
            return DateTime.SpecifyKind(result.Date, kind);
        }
    }
}