using System.Threading;
using System.Threading.Tasks;

using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor;

internal interface IConsoleInputGetter
{
    Task<KeyInput> ReadKeyWhileBlinkLine(
        InProgressLine line,
        bool currentLineIsValid,
        CancellationToken cancellationToken);
}