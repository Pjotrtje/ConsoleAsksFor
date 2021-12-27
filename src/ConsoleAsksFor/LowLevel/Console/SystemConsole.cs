using C = System.Console;

namespace ConsoleAsksFor;

internal sealed class SystemConsole : ISystemConsole
{
    public bool CursorVisible
    {
        set => C.CursorVisible = value;
    }

    public Position CursorPosition
    {
        get => new(C.CursorLeft, C.CursorTop);
        set => C.SetCursorPosition(value.Left, value.Top);
    }

    public int WindowWidth => C.WindowWidth;

    public async Task<ConsoleKeyInfo> ReadKey(CancellationToken cancellationToken)
        => await KeyReader.ReadKey(cancellationToken);

    // https://stackoverflow.com/questions/57615/how-to-add-a-timeout-to-console-readline/18342182#18342182
    private static class KeyReader
    {
        private static readonly AutoResetEvent GetInput = new(false);
        private static readonly AsyncAutoResetEvent GotInput = new(false);
        private static ConsoleKeyInfo input;

        static KeyReader()
        {
            var t = new Thread(Read)
            {
                IsBackground = true,
            };
            t.Start();
        }

        private static void Read()
        {
            while (true)
            {
                GetInput.WaitOne();
                input = C.ReadKey(true);
                GotInput.Set();
            }
        }

        public static async Task<ConsoleKeyInfo> ReadKey(CancellationToken cancellationToken)
        {
            GetInput.Set();
            await GotInput.WaitAsync(cancellationToken);
            return input;
        }
    }
}