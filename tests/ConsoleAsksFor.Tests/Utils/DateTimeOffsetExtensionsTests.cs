using System;

using FluentAssertions;
using FluentAssertions.Extensions;

using Xunit;

namespace ConsoleAsksFor.Tests
{
    public class DateTimeOffsetExtensionsTests
    {
        private static readonly TimeZoneInfo WestEuropeStandardTime = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

        public static TheoryData<DateTimeOffset, TimeZoneInfo, DateTimeOffset, string> ToTimeZoneUseCases()
            => new()
            {
                {
                    new DateTimeOffset(1.January(2020).At(09, 30), TimeSpan.FromHours(3)),
                    TimeZoneInfo.Utc,
                    new DateTimeOffset(1.January(2020).At(06, 30).AsUtc(), TimeSpan.Zero),
                    "From -3 To Utc"
                },
                {
                    new DateTimeOffset(1.January(2020).At(09, 30), TimeSpan.FromHours(3)),
                    WestEuropeStandardTime,
                    new DateTimeOffset(1.January(2020).At(07, 30), TimeSpan.FromHours(1)),
                    "From -3 To WestEuropeStandardTime"
                },
                {
                    new DateTimeOffset(1.January(2020).At(06, 30).AsUtc(), TimeSpan.Zero),
                    TimeZoneInfo.Utc,
                    new DateTimeOffset(1.January(2020).At(06, 30).AsUtc(), TimeSpan.Zero),
                    "From Utc To Utc"
                },
                {
                    new DateTimeOffset(1.January(2020).At(7, 30), TimeSpan.FromHours(1)),
                    WestEuropeStandardTime,
                    new DateTimeOffset(1.January(2020).At(7, 30), TimeSpan.FromHours(1)),
                    "From WestEuropeStandardTime To WestEuropeStandardTime"
                },
            };

        [Theory]
        [MemberData(nameof(ToTimeZoneUseCases))]
        public void ToTimeZone_Works_Correct(DateTimeOffset input, TimeZoneInfo timeZone, DateTimeOffset expectedResult, string useCase)
        {
            var dateTimeOffset = input.ToTimeZone(timeZone);

            dateTimeOffset.Should().Be(expectedResult, useCase);
        }

        public static TheoryData<DateTimeOffset, long, DateTimeOffset, string> TruncateMinValueUseCases()
            => new()
            {
                {
                    new DateTimeOffset(20.January(2020).At(09, 30, 34).AsUtc(), TimeSpan.Zero).AddMilliseconds(10),
                    TimeSpan.TicksPerSecond,
                    new DateTimeOffset(20.January(2020).At(09, 30, 35).AsUtc(), TimeSpan.Zero),
                    "UTC - Remove Milliseconds"
                },
                {
                    new DateTimeOffset(20.January(2020).At(09, 30, 34).AsUtc(), TimeSpan.Zero).AddMilliseconds(10),
                    TimeSpan.TicksPerDay,
                    new DateTimeOffset(21.January(2020).At(00, 00).AsUtc(), TimeSpan.Zero),
                    "UTC - Remove Time"
                },
                {
                    new DateTimeOffset(20.January(2020).At(00, 00).AsUtc(), TimeSpan.Zero),
                    TimeSpan.TicksPerDay,
                    new DateTimeOffset(20.January(2020).At(00, 00).AsUtc(), TimeSpan.Zero),
                    "UTC: Nothing to remove"
                },
                {
                    new DateTimeOffset(20.January(2020).At(09, 30, 34), TimeSpan.FromHours(2)).AddMilliseconds(10),
                    TimeSpan.TicksPerSecond,
                    new DateTimeOffset(20.January(2020).At(09, 30, 35), TimeSpan.FromHours(2)),
                    "Zoned - Remove Milliseconds"
                },
                {
                    new DateTimeOffset(20.January(2020).At(09, 30, 34), TimeSpan.FromHours(2)).AddMilliseconds(10),
                    TimeSpan.TicksPerDay,
                    new DateTimeOffset(21.January(2020).At(00, 00), TimeSpan.FromHours(2)),
                    "Zoned: Remove Time"
                },
                {
                    new DateTimeOffset(20.January(2020).At(00, 00), TimeSpan.FromHours(2)),
                    TimeSpan.TicksPerDay,
                    new DateTimeOffset(20.January(2020).At(00, 00), TimeSpan.FromHours(2)),
                    "Zoned: Nothing to remove"
                },
            };

        [Theory]
        [MemberData(nameof(TruncateMinValueUseCases))]
        public void TruncateMinValue(DateTimeOffset input, long resolution, DateTimeOffset expectedResult, string useCase)
        {
            var dateTimeOffset = input.TruncateMinValue(resolution);

            dateTimeOffset.Should().Be(expectedResult, useCase);
        }

        public static TheoryData<DateTimeOffset, long, DateTimeOffset, string> TruncateMaxValueUseCases()
            => new()
            {
                {
                    new DateTimeOffset(20.January(2020).At(09, 30, 34).AsUtc(), TimeSpan.Zero).AddMilliseconds(10),
                    TimeSpan.TicksPerSecond,
                    new DateTimeOffset(20.January(2020).At(09, 30, 34).AsUtc(), TimeSpan.Zero),
                    "UTC - Remove Milliseconds"
                },
                {
                    new DateTimeOffset(20.January(2020).At(09, 30, 34).AsUtc(), TimeSpan.Zero).AddMilliseconds(10),
                    TimeSpan.TicksPerDay,
                    new DateTimeOffset(20.January(2020).At(00, 00).AsUtc(), TimeSpan.Zero),
                    "UTC - Remove Time"
                },
                {
                    new DateTimeOffset(20.January(2020).At(00, 00).AsUtc(), TimeSpan.Zero),
                    TimeSpan.TicksPerDay,
                    new DateTimeOffset(20.January(2020).At(00, 00).AsUtc(), TimeSpan.Zero),
                    "UTC: Nothing to remove"
                },
                {
                    new DateTimeOffset(20.January(2020).At(09, 30, 34), TimeSpan.FromHours(2)).AddMilliseconds(10),
                    TimeSpan.TicksPerSecond,
                    new DateTimeOffset(20.January(2020).At(09, 30, 34), TimeSpan.FromHours(2)),
                    "Zoned - Remove Milliseconds"
                },
                {
                    new DateTimeOffset(20.January(2020).At(09, 30, 34), TimeSpan.FromHours(2)).AddMilliseconds(10),
                    TimeSpan.TicksPerDay,
                    new DateTimeOffset(20.January(2020).At(00, 00), TimeSpan.FromHours(2)),
                    "Zoned: Remove Time"
                },
                {
                    new DateTimeOffset(20.January(2020).At(00, 00), TimeSpan.FromHours(2)),
                    TimeSpan.TicksPerDay,
                    new DateTimeOffset(20.January(2020).At(00, 00), TimeSpan.FromHours(2)),
                    "Zoned: Nothing to remove"
                },
            };

        [Theory]
        [MemberData(nameof(TruncateMaxValueUseCases))]
        public void TruncateMaxValue(DateTimeOffset input, long resolution, DateTimeOffset expectedResult, string useCase)
        {
            var dateTimeOffset = input.TruncateMaxValue(resolution);

            dateTimeOffset.Should().Be(expectedResult, useCase);
        }
    }
}