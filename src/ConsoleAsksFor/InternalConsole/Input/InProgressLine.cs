namespace ConsoleAsksFor;

internal sealed record InProgressLine
{
    public string Value { get; init; }
    public string IntellisenseHint { get; init; }

    public int CursorIndex { get; init; }
    public bool MustObfuscate { get; init; }

    public InProgressLine(string value, bool mustObfuscate)
    {
        Value = value;
        IntellisenseHint = "";
        CursorIndex = value.Length;
        MustObfuscate = mustObfuscate;
    }

    public string DisplayValue
        => MustObfuscate
            ? new string('*', Value.Length)
            : Value;

    public int Length => Value.Length;

    public override string ToString()
        => $"{DisplayValue} ({nameof(CursorIndex)}={CursorIndex})";
}