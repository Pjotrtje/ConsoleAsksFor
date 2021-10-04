using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAsksFor
{
    internal interface ISystemConsole
    {
        bool CursorVisible { set; }

        Position CursorPosition { get; set; }

        int WindowWidth { get; }

        Task<ConsoleKeyInfo> ReadKey(CancellationToken cancellationToken);
    }
}