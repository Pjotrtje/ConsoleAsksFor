namespace ConsoleAsksFor.NodaTime.ISO;

internal sealed class LocalDateTimeFormat
{
    public LocalDateTimePattern Pattern { get; }

    public IReadOnlyCollection<string> IntellisenseMinPatterns { get; }

    public IReadOnlyCollection<string> IntellisenseMaxPatterns { get; }

    public Period SmallestIncrement { get; }

    private LocalDateTimeFormat(
        string pattern,
        IReadOnlyCollection<string> intellisenseMinPatterns,
        IReadOnlyCollection<string> intellisenseMaxPatterns,
        Period smallestIncrement)
    {
        Pattern = LocalDateTimePattern.CreateWithInvariantCulture(pattern);
        IntellisenseMinPatterns = intellisenseMinPatterns;
        IntellisenseMaxPatterns = intellisenseMaxPatterns;
        SmallestIncrement = smallestIncrement;
    }

    public string FormatAnswer(LocalDateTime value)
        => value.ToString(Pattern.PatternText, CultureInfo.InvariantCulture);

    public static readonly LocalDateTimeFormat DateTime = new(
        "uuuu-MM-dd HH:mm:ss",
        new[]
        {
            "-X999-01-01 00:00:00",
            "-XXXX-X0-01 00:00:00",
            "-XXXX-XX-X0 00:00:00",
            "-9998-01-01 00:00:00",
            "XXXX-XX-X0 00:00:00",
            "XXXX-X0-01 00:00:00",
            "X000-01-01 00:00:00",
            "0001-01-01 00:00:00",
        },
        new[]
        {
            "-XXXX-X9-30 23:59:59",
            "-XXXX-XX-29 23:59:59",
            "-XXXX-XX-28 23:59:59",
            "-X000-12-31 23:59:59",
            "-0001-12-31 23:59:59",
            "-XXXX-XX-XX X9:59:59",
            "XXXX-XX-XX X9:59:59",
            "XXXX-X9-30 23:59:59",
            "XXXX-XX-29 23:59:59",
            "XXXX-XX-28 23:59:59",
            "9999-12-31 23:59:59",
        },
        Period.FromSeconds(1));

    public static readonly LocalDateTimeFormat Date = new(
        "uuuu-MM-dd",
        new[]
        {
            "-X999-01-01",
            "-XXXX-X0-01",
            "-XXXX-XX-X0",
            "-9998-01-01",
            "XXXX-XX-X0",
            "XXXX-X0-01",
            "X000-01-01",
            "0001-01-01",
        },
        new[]
        {
            "-XXXX-X9-30",
            "-XXXX-XX-29",
            "-XXXX-XX-28",
            "-X000-12-31",
            "-0001-12-31",
            "XXXX-X9-30",
            "XXXX-XX-29",
            "XXXX-XX-28",
            "9999-12-31",
        },
        Period.FromDays(1));

    public static readonly LocalDateTimeFormat YearMonth = new(
        "uuuu-MM",
        new[]
        {
            "-X999-01",
            "-9998-01",
            "X000-01",
            "0001-01",
        },
        new[]
        {
            "-XXXX-X9",
            "-X000-12",
            "-0001-12",
            "XXXX-X9",
            "9999-12",
        },
        Period.FromMonths(1));

    public static readonly LocalDateTimeFormat AnnualDate = new(
        "MM-dd",
        new[]
        {
            "01-01",
            "X0-01",
            "XX-X0",
        },
        new[]
        {
            "X9-30",
            "XX-29",
            "XX-28",
            "12-31",
        },
        Period.FromDays(1));

    public static readonly LocalDateTimeFormat Time = new(
        "HH:mm:ss",
        new[]
        {
            "00:00:00",
        },
        new[]
        {
            "23:59:59",
        },
        Period.FromSeconds(1));
}