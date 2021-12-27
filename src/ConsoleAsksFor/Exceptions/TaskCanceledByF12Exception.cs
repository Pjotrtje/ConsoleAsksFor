using System.Threading.Tasks;

namespace ConsoleAsksFor;

/// <summary>
/// Represents an exception used to communicate task cancellation by pressing F12.
/// </summary>
public sealed class TaskCanceledByF12Exception : TaskCanceledException
{
}