namespace ConsoleAsksFor.Tests;

public class DateTimeOffsetQuestionIntellisenseTests
{
    private readonly DateTimeOffsetQuestionIntellisense _sut = new(
        new DateTimeOffsetQuestionParser(
            DateTimeOffsetFormat.Date,
            TimeZoneInfo.Utc,
            RangeConstraint
                .Between(
                    2.January(1999).AsUtc().ToDateTimeOffset(),
                    4.May(4000).AsUtc().ToDateTimeOffset())
                .ToClusteredRange(TimeZoneInfo.Utc, DateTimeOffsetFormat.Date)),
        DateTimeOffsetFormat.Date);

    private static readonly IntellisenseUseCases UseCases = new()
    {
        FromUser = new List<IntellisenseUseCases.FromUserInput>
        {
            new() { Input = "",           Previous = "4000-05-04", Next = "1999-01-02", UseCase = "No value" },
            new() { Input = " ",          Previous = "4000-05-04", Next = "1999-01-02", UseCase = "Only whitespace" },
            new() { Input = "2",          Previous = "2999-12-31", Next = "2000-01-01", UseCase = "Incomplete 1 digits" },
            new() { Input = " 2 ",        Previous = "2999-12-31", Next = "2000-01-01", UseCase = "Incomplete 1 digits with whitespace" },
            new() { Input = "20",         Previous = "2099-12-31", Next = "2000-01-01", UseCase = "Incomplete 2 digits" },
            new() { Input = "200",        Previous = "2009-12-31", Next = "2000-01-01", UseCase = "Incomplete 3 digits" },
            new() { Input = "2000",       Previous = "2000-12-31", Next = "2000-01-01", UseCase = "Incomplete 4 digits" },
            new() { Input = "2000-",      Previous = "2000-12-31", Next = "2000-01-01", UseCase = "Incomplete 5 digits" },
            new() { Input = "2000-0",     Previous = "2000-09-30", Next = "2000-01-01", UseCase = "Incomplete 6 digits - january/september" },
            new() { Input = "2000-1",     Previous = "2000-12-31", Next = "2000-10-01", UseCase = "Incomplete 6 digits - october/december" },
            new() { Input = "2000-01",    Previous = "2000-01-31", Next = "2000-01-01", UseCase = "Incomplete 7 digits" },
            new() { Input = "2000-01-",   Previous = "2000-01-31", Next = "2000-01-01", UseCase = "Incomplete 8 digits" },
            new() { Input = "2000-01-0",  Previous = "2000-01-09", Next = "2000-01-01", UseCase = "Incomplete 9 digits - first 9 days" },
            new() { Input = "2000-01-1",  Previous = "2000-01-19", Next = "2000-01-10", UseCase = "Incomplete XX digits - 10 next days" },
            new() { Input = "2000-01-10", Previous = null,         Next = null,         UseCase = "Valid value" },
            new() { Input = "9",          Previous = null,         Next = null,         UseCase = "Incomplete value which is never in range" },
            new() { Input = "lala",       Previous = null,         Next = null,         UseCase = "Invalid value" },
        },
        FromIntellisense = new List<IntellisenseUseCases.FromIntellisenseInput>
        {
            new() { Input = "2000-01-01",   Hint = "2000-01-0", Previous = "2000-01-09", Next = "2000-01-02", UseCase = "1 day digit missing - filled (min) value" },
            new() { Input = "2000-01-09",   Hint = "2000-01-0", Previous = "2000-01-08", Next = "2000-01-01", UseCase = "1 day digit missing - Filled (max) value" },
            new() { Input = "2000-01-01",   Hint = "2000-01-",  Previous = "2000-01-31", Next = "2000-01-02", UseCase = "2 day digit missing - filled (min) value" },
            new() { Input = "2000-01-31",   Hint = "2000-01-",  Previous = "2000-01-30", Next = "2000-01-01", UseCase = "2 day digit missing - Filled (max) value" },
            new() { Input = "2000-02-01",   Hint = "2000-02-",  Previous = "2000-02-29", Next = "2000-02-02", UseCase = "2 day digit missing (february-leap) - filled (min) value" },
            new() { Input = "2001-02-01",   Hint = "2001-02-",  Previous = "2001-02-28", Next = "2001-02-02", UseCase = "2 day digit missing (february-nonleap) - filled (min) value" },
            new() { Input = "2000-02-29",   Hint = "2000-02-",  Previous = "2000-02-28", Next = "2000-02-01", UseCase = "2 day digit missing (february-leap) - filled (max) value" },
            new() { Input = "2001-02-28",   Hint = "2001-02-",  Previous = "2001-02-27", Next = "2001-02-01", UseCase = "2 day digit missing (february-nonleap) - filled (max) value" },
            new() { Input = "2000-01-01",   Hint = "2000-0",    Previous = "2000-09-30", Next = "2000-01-02", UseCase = "1 month digit missing (start with 0) - filled (min) value" },
            new() { Input = "2000-09-30",   Hint = "2000-0",    Previous = "2000-09-29", Next = "2000-01-01", UseCase = "1 month digit missing (start with 0) - Filled (max) value" },
            new() { Input = "2000-10-01",   Hint = "2000-1",    Previous = "2000-12-31", Next = "2000-10-02", UseCase = "1 month digit missing (start with 1) - filled (min) value" },
            new() { Input = "2000-12-31",   Hint = "2000-1",    Previous = "2000-12-30", Next = "2000-10-01", UseCase = "1 month digit missing (start with 1) - Filled (max) value" },
            new() { Input = "2000-01-01",   Hint = "2000-",     Previous = "2000-12-31", Next = "2000-01-02", UseCase = "2 month digit missing - filled (min) value" },
            new() { Input = "2000-12-31",   Hint = "2000-",     Previous = "2000-12-30", Next = "2000-01-01", UseCase = "2 month digit missing - Filled (max) value" },
            new() { Input = "2000-12-31",   Hint = " 2000- ",   Previous = "2000-12-30", Next = "2000-01-01", UseCase = "2 month digit missing - Filled (max) value with whitespace around hint" },
            new() { Input = " 2000-12-31 ", Hint = "2000-",     Previous = "2000-12-30", Next = "2000-01-01", UseCase = "2 month digit missing - Filled (max) value with whitespace around input" },
        },
    };

    public static TheoryData<string, string, string?> CompleteValueUseCases => UseCases.CompleteValueUseCases;

    public static TheoryData<string, string, string, string?> PreviousValueUseCases => UseCases.PreviousValueUseCases;

    public static TheoryData<string, string, string, string?> NextValueUseCases => UseCases.NextValueUseCases;

    [Theory]
    [MemberData(nameof(CompleteValueUseCases))]
    public void CompleteValue_Returns_CorrectValue(string useCase, string value, string? newValue)
    {
        _sut.CompleteValue(value).Should().Be(newValue, useCase);
    }

    [Theory]
    [MemberData(nameof(PreviousValueUseCases))]
    public void PreviousValue_Returns_CorrectValue(string useCase, string value, string hint, string? newValue)
    {
        _sut.PreviousValue(value, hint).Should().Be(newValue, useCase);
    }

    [Theory]
    [MemberData(nameof(NextValueUseCases))]
    public void NextValue_Returns_CorrectValue(string useCase, string value, string hint, string? newValue)
    {
        _sut.NextValue(value, hint).Should().Be(newValue, useCase);
    }

    [Fact]
    public void Handles_TimeZone_When_MinValue_Is_Not_0001_01_01_00_00_00_Due_To_Offset()
    {
        //ToDo op ander nivo testen
        var westEuropeStandardTime = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
        var sut = new DateTimeOffsetQuestionIntellisense(
            new DateTimeOffsetQuestionParser(
                DateTimeOffsetFormat.DateTime,
                westEuropeStandardTime,
                AskForAppender.ToClusteredRange(RangeConstraint.None, westEuropeStandardTime, DateTimeOffsetFormat.DateTime)),
            DateTimeOffsetFormat.DateTime);

        sut.CompleteValue("").Should().Be("0001-01-01 01:00:00");
        sut.NextValue("", "").Should().Be("0001-01-01 01:00:00");
    }

    [Fact]
    public void Handles_TimeZone_When_MaxValue_Is_Not_9999_12_31_13_59_59_Due_To_Offset()
    {
        //ToDo op ander nivo testen
        var hawaiianTime = TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time");
        var sut = new DateTimeOffsetQuestionIntellisense(
            new DateTimeOffsetQuestionParser(
                DateTimeOffsetFormat.DateTime,
                hawaiianTime,
                AskForAppender.ToClusteredRange(RangeConstraint.None, hawaiianTime, DateTimeOffsetFormat.DateTime)),
            DateTimeOffsetFormat.DateTime);

        sut.PreviousValue("", "").Should().Be("9999-12-31 13:59:59");
    }
}