using System.Linq;

namespace ConsoleAsksFor;

internal sealed class QuestionItem
{
    public const string StringEmptyReplacement = "<EMPTY>";
    public string RealValue { get; }
    public string Display { get; }
    public bool IsTrimmed { get; }
    public bool HasEscapedWhitespace { get; }
    public bool HasEscapedSplitter { get; }
    public bool HasRemovedNotPrintableChars { get; }
    public bool IsReplacedStringEmpty { get; }

    private static string WithVisibleWhitespace(string str)
        => str
            .Replace("\t", @"\t")
            .Replace("\n", @"\n")
            .Replace("\f", @"\f")
            .Replace("\v", @"\v")
            .Replace("\r", @"\r");

    private static string WithEscapedSplitter(string str)
        => str
            .Replace(Splitter.Value.ToString(), Splitter.EscapedValue);

    private static string WithoutNotPrintableChars(string str)
        => str
            .Where(c => c.CanBeTypedInConsoleAsksFor())
            .JoinToString();

    public QuestionItem(string item, bool escapeSplitter)
    {
        var trimmed = item.Trim();
        IsTrimmed = item != trimmed;

        var withEscapedSplitter = escapeSplitter
            ? WithEscapedSplitter(trimmed)
            : trimmed;
        HasEscapedSplitter = escapeSplitter && trimmed != withEscapedSplitter;

        var escapedWhitespace = WithVisibleWhitespace(withEscapedSplitter);
        HasEscapedWhitespace = withEscapedSplitter != escapedWhitespace;

        var withoutNotPrintableChars = WithoutNotPrintableChars(escapedWhitespace);
        HasRemovedNotPrintableChars = escapedWhitespace != withoutNotPrintableChars;

        var withReplacedStringEmpty = withoutNotPrintableChars == string.Empty
            ? StringEmptyReplacement
            : withoutNotPrintableChars;
        IsReplacedStringEmpty = withReplacedStringEmpty != withoutNotPrintableChars;

        RealValue = item;
        Display = withReplacedStringEmpty;
    }
}