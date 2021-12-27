namespace ConsoleAsksFor;

internal interface IConsoleInputGetter
{
    Task<KeyInput> ReadKeyWhileBlinkLine(
        InProgressLine line,
        bool currentLineIsValid,
        CancellationToken cancellationToken);
}