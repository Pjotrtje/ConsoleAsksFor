namespace ConsoleAsksFor.NodaTime.ISO.Tests;

public class LocalDateTimeQuestionIntellisenseTests
{
    private readonly LocalDateTimeQuestionIntellisense _sut = new(
        new LocalDateTimeQuestionParser(
            LocalDateTimeFormat.Date,
            DateTimeZone.Utc,
            RangeConstraint.Between(
                new LocalDateTime(1999, 01, 02, 00, 00),
                new LocalDateTime(4000, 05, 04, 00, 00))),
        LocalDateTimeFormat.Date);

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
}