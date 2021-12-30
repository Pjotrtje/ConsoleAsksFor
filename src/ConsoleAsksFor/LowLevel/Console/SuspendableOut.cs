namespace ConsoleAsksFor;

internal sealed class SuspendableOut : TextWriter, ISuspendableOut, ISuspendableOutWriter
{
    private readonly ISystemConsole _systemConsole;
    private readonly IDirectOut _directOut;
    private readonly object _lockObject = new();
    private readonly List<Action> _delayedActions = new();

    public override Encoding Encoding => _directOut.Encoding;

    private bool _isLocked;

    public SuspendableOut(
        ISystemConsole systemConsole,
        IDirectOut directOut)
    {
        _systemConsole = systemConsole;
        _directOut = directOut;
    }

    public void Suspend()
    {
        lock (_lockObject)
        {
            if (_isLocked)
            {
                return;
            }

            _isLocked = true;
            if (_systemConsole.CursorPosition.Left != 0)
            {
                _directOut.WriteLine("");
            }
        }
    }

    public void Resume()
    {
        lock (_lockObject)
        {
            if (!_isLocked)
            {
                return;
            }

            foreach (var delayedAction in _delayedActions)
            {
                delayedAction();
            }

            _delayedActions.Clear();
            if (_systemConsole.CursorPosition.Left != 0)
            {
                _directOut.WriteLine("");
            }
            _isLocked = false;
        }
    }

    public override void Write(char value)
        => DoLocked(() => _directOut.Write(value));

    public override void Write(string? value)
        => DoLocked(() => _directOut.Write(value));

    public override void WriteLine(string? value)
        => DoLocked(() => _directOut.WriteLine(value));

    private void DoLocked(Action action)
    {
        lock (_lockObject)
        {
            if (_isLocked)
            {
                _delayedActions.Add(action);
            }
            else
            {
                action();
            }
        }
    }
}