namespace ConsoleAsksFor;

internal interface ISystemConsole : IWindowWidthProvider
{
    bool CursorVisible { set; }

    Position CursorPosition { get; set; }

    Task<ConsoleKeyInfo> ReadKey(CancellationToken cancellationToken);
}

internal interface IWindowWidthProvider
{
    int WindowWidth { get; }
}
