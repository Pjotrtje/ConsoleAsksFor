namespace ConsoleAsksFor;

internal interface ISystemConsole
{
    bool CursorVisible { set; }

    Position CursorPosition { get; set; }

    int WindowWidth { get; }

    Task<ConsoleKeyInfo> ReadKey(CancellationToken cancellationToken);
}