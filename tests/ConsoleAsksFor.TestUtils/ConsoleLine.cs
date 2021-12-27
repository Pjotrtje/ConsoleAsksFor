namespace ConsoleAsksFor.TestUtils;

public sealed record ConsoleLine(
    LineTypeId LineTypeId,
    string? Line)
{
    public override string ToString()
        => $"{LineTypeId}:{Line}";
}