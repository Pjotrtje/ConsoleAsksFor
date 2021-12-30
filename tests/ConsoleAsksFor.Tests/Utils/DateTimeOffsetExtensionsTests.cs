namespace ConsoleAsksFor.Tests;

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
}