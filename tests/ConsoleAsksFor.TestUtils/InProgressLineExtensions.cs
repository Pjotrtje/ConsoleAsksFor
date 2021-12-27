namespace ConsoleAsksFor.TestUtils;

internal static class InProgressLineExtensions
{
    public static InProgressLine AtIndex(this InProgressLine line, int cursorIndex)
        => line with
        {
            CursorIndex = cursorIndex,
        };
}