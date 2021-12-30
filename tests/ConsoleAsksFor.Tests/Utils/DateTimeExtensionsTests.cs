namespace ConsoleAsksFor.Tests;

public class DateTimeExtensionsTests
{
    private static readonly TimeZoneInfo WestEuropeStandardTime = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

    public static TheoryData<DateTime, TimeZoneInfo, DateTimeOffset, string> ToDateTimeOffsetUseCases()
    {
        var momentBeforeSummerTime = 28.March(2021).At(01, 59);
        var momentAfterSummerTime = 28.March(2021).At(03, 00);
        var momentBeforeWinterTime = 31.October(2021).At(01, 59);
        var momentAfterWinterTime = 31.October(2021).At(02, 00);

        return new()
        {
            { momentBeforeSummerTime, WestEuropeStandardTime, new DateTimeOffset(momentBeforeSummerTime, 1.Hours()), "moment before summer time" },
            { momentAfterSummerTime, WestEuropeStandardTime, new DateTimeOffset(momentAfterSummerTime, 2.Hours()), "moment after summer time" },
            { momentBeforeWinterTime, WestEuropeStandardTime, new DateTimeOffset(momentBeforeWinterTime, 2.Hours()), "moment before winter time" },
            { momentAfterWinterTime, WestEuropeStandardTime, new DateTimeOffset(momentAfterWinterTime, 1.Hours()), "moment after winter time (map to second time time occurs)" },

            { momentBeforeSummerTime, TimeZoneInfo.Utc, new DateTimeOffset(momentBeforeSummerTime, 0.Hours()), "moment before summer time" },
            { momentAfterSummerTime, TimeZoneInfo.Utc, new DateTimeOffset(momentAfterSummerTime, 0.Hours()), "moment after summer time" },
            { momentBeforeWinterTime, TimeZoneInfo.Utc, new DateTimeOffset(momentBeforeWinterTime, 0.Hours()), "moment before winter time" },
            { momentAfterWinterTime, TimeZoneInfo.Utc, new DateTimeOffset(momentAfterWinterTime, 0.Hours()), "moment after winter time (map to second time time occurs)" },
        };
    }

    [Theory]
    [MemberData(nameof(ToDateTimeOffsetUseCases))]
    public void When_Date_Exists_ToDateTimeOffset_Maps_Correct(
        DateTime unspecifiedDateTime,
        TimeZoneInfo timeZone,
        DateTimeOffset expectedResult,
        string useCase)
    {
        var result = unspecifiedDateTime.ToDateTimeOffset(timeZone);
        result.Should().Be(expectedResult, useCase);
    }

    [Fact]
    public void When_Date_Does_Not_Exist_ToDateTimeOffset_Throws()
    {
        var notExistingDateTime = 28.March(2021).At(02, 00);
        Action toDateTimeOffset = () => notExistingDateTime.ToDateTimeOffset(WestEuropeStandardTime);
        toDateTimeOffset.Should().ThrowExactly<ArgumentException>().WithMessage("The supplied DateTime represents an invalid time.*");
    }

    [Theory]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Utc)]
    public void When_Specified_Kind_ToDateTimeOffset_Throws(DateTimeKind kind)
    {
        var specifiedDateTime = DateTime.SpecifyKind(1.January(2020), kind);
        Action action = () => specifiedDateTime.ToDateTimeOffset(TimeZoneInfo.Local);
        action.Should().Throw<ArgumentException>().WithMessage("Expected Kind to be Unspecified*");
    }

    public static TheoryData<DateTime, DateTimeKind, DateTime, string> ToKindUseCases()
        => new()
        {
            { 1.January(2020).AsLocal(), DateTimeKind.Local, 1.January(2020).AsLocal(), "Date=Local, Kind=Local" },
            { 1.January(2020), DateTimeKind.Local, 1.January(2020).AsLocal(), "Date=Unspecified, Kind=Local" },
            { 1.January(2020).AsUtc(), DateTimeKind.Local, 1.January(2020).At(01, 00).AsLocal(), "Date=Utc, Kind=Local" },

            { 2.January(2020).AsLocal(), DateTimeKind.Utc, 1.January(2020).At(23, 00).AsUtc(), "Date=Local, Kind=Utc" },
            { 2.January(2020), DateTimeKind.Utc, 1.January(2020).At(23, 00).AsUtc(), "Date=Unspecified, Kind=Utc" },
            { 2.January(2020).AsUtc(), DateTimeKind.Utc, 2.January(2020).AsUtc(), "Date=Utc, Kind=Utc" },
        };

    [Trait(Trait.Category, Trait.IgnoreInDevOps)] // Depends to much on TimeZoneInfo.Local
    [Theory]
    [MemberData(nameof(ToKindUseCases))]
    public void When_Specified_Kind_ToKind_Returns_Correct_Value(DateTime input, DateTimeKind kind, DateTime expected, string useCase)
    {
        var result = input.ToKind(kind);
        result.Should().Be(expected, useCase).And.BeIn(expected.Kind);
    }

    [Fact]
    public void When_Unspecified_Kind_ToKind_Throws()
    {
        var specifiedDateTime = DateTime.SpecifyKind(1.January(2020), DateTimeKind.Local);
        Action action = () => specifiedDateTime.ToKind(DateTimeKind.Unspecified);
        action.Should().Throw<ArgumentException>().WithMessage("Expected value not to be Unspecified*");
    }
}