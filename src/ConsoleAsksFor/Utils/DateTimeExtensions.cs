using System;

namespace ConsoleAsksFor
{
    internal static class DateTimeExtensions
    {
        public static DateTimeOffset ToDateTimeOffset(this DateTime unspecifiedDateTime, TimeZoneInfo timeZone)
        {
            if (unspecifiedDateTime.Kind != DateTimeKind.Unspecified)
            {
                throw new ArgumentException($"Expected {nameof(DateTime.Kind)} to be {DateTimeKind.Unspecified} but found {unspecifiedDateTime.Kind}.", nameof(unspecifiedDateTime));
            }

            var utcDateTime = TimeZoneInfo.ConvertTimeToUtc(unspecifiedDateTime, timeZone);
            var offset = timeZone.GetUtcOffset(utcDateTime);
            return new DateTimeOffset(unspecifiedDateTime, offset);
        }

        public static DateTime ToKind(this DateTime dateTime, DateTimeKind kind)
        {
            if (kind == DateTimeKind.Unspecified)
            {
                throw new ArgumentException($"Expected value not to be {DateTimeKind.Unspecified} but found {DateTimeKind.Unspecified}.", nameof(kind));
            }

            return kind == DateTimeKind.Utc
                ? dateTime.ToUtc()
                : dateTime.ToLocal();
        }

        private static DateTime ToLocal(this DateTime dateTime)
            => dateTime.Kind == DateTimeKind.Utc
                ? dateTime.ToLocalTime()
                : DateTime.SpecifyKind(dateTime, DateTimeKind.Local);

        private static DateTime ToUtc(this DateTime dateTime)
            => dateTime.Kind == DateTimeKind.Utc
                ? dateTime
                : DateTime.SpecifyKind(dateTime, DateTimeKind.Local).ToUniversalTime();
    }
}