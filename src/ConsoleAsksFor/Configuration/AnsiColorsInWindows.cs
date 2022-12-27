namespace ConsoleAsksFor;

// https://www.jerriepelser.com/blog/using-ansi-color-codes-in-net-console-apps/
internal static partial class AnsiColorsInWindows
{
    private const int StdOutputHandle = -11;
    private const uint EnableVirtualTerminalProcessing = 0x0004;

    public static void Enable()
    {
        var stdHandle = GetStdHandle(StdOutputHandle);
        if (GetConsoleMode(stdHandle, out var outConsoleMode))
        {
            SetConsoleMode(stdHandle, outConsoleMode | EnableVirtualTerminalProcessing);
        }
    }

#if NET7_0_OR_GREATER

    [LibraryImport("kernel32.dll", SetLastError = true)]
    private static partial nint GetStdHandle(int nStdHandle);

    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetConsoleMode(nint hConsoleHandle, out uint lpMode);

    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetConsoleMode(nint hConsoleHandle, uint dwMode);

#else
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern nint GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll")]
    private static extern bool GetConsoleMode(nint hConsoleHandle, out uint lpMode);

    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleMode(nint hConsoleHandle, uint dwMode);
#endif
}
