using System;
using System.Runtime.InteropServices;

namespace ConsoleAsksFor
{
    // https://www.jerriepelser.com/blog/using-ansi-color-codes-in-net-console-apps/
    internal static class AnsiColorsInWindows
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

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
    }
}