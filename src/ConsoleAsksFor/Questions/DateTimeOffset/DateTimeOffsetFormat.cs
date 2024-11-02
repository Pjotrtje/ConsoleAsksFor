namespace ConsoleAsksFor;

internal sealed class DateTimeOffsetFormat
{
    public string Pattern { get; }

    public string PatternIncludingPrefix { get; }

    public string Prefix { get; }

    public IReadOnlyCollection<string> IntellisenseMinPatterns { get; }

    public IReadOnlyCollection<string> IntellisenseMaxPatterns { get; }

    public long SmallestIncrementInTicks { get; }

    private DateTimeOffsetFormat(
        string pattern,
        string patternIncludingPrefix,
        string prefix,
        IReadOnlyCollection<string> intellisenseMinPatterns,
        IReadOnlyCollection<string> intellisenseMaxPatterns,
        long smallestIncrementInTicks)
    {
        Pattern = pattern;
        PatternIncludingPrefix = patternIncludingPrefix;
        Prefix = prefix;
        IntellisenseMinPatterns = intellisenseMinPatterns;
        IntellisenseMaxPatterns = intellisenseMaxPatterns;
        SmallestIncrementInTicks = smallestIncrementInTicks;
    }

    public string FormatAnswer(DateTimeOffset value)
        => value.ToString(Pattern, CultureInfo.InvariantCulture);

    public static readonly DateTimeOffsetFormat Time = new(
        "HH:mm:ss",
        "yyyy-MM-dd HH:mm:ss",
        "2000-01-01 ",
        [
            "00:00:00",
        ],
        [
            "23:59:59",
        ],
        TimeSpan.TicksPerSecond);

    public static readonly DateTimeOffsetFormat DateTime = new(
        "yyyy-MM-dd HH:mm:ss",
        "yyyy-MM-dd HH:mm:ss",
        "",
        [
            "XXXX-XX-X0 00:00:00",
            "XXXX-X0-01 00:00:00",
            "X000-01-01 00:00:00",
            "0001-01-01 00:00:00",
        ],
        [
            "XXXX-XX-XX X9:59:59",
            "XXXX-X9-30 23:59:59",
            "XXXX-XX-29 23:59:59",
            "XXXX-XX-28 23:59:59",
            "9999-12-31 23:59:59",
        ],
        TimeSpan.TicksPerSecond);

    public static readonly DateTimeOffsetFormat Date = new(
        "yyyy-MM-dd",
        "yyyy-MM-dd",
        "",
        [
            "XXXX-XX-X0",
            "XXXX-X0-01",
            "X000-01-01",
            "0001-01-01",
        ],
        [
            "XXXX-X9-30",
            "XXXX-XX-29",
            "XXXX-XX-28",
            "9999-12-31",
        ],
        TimeSpan.TicksPerDay);
}