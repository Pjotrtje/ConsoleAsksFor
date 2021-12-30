namespace ConsoleAsksFor;

internal sealed class OutSuspender : IOutSuspender
{
    private sealed class Suspender : IDisposable
    {
        private readonly ISuspendableOut _regular;
        private readonly ISuspendableOut _error;

        public Suspender(
            ISuspendableOut regular,
            ISuspendableOut error)
        {
            _regular = regular;
            _error = error;

            _regular.Suspend();
            _error.Suspend();
        }

        public void Dispose()
        {
            _regular.Resume();
            _error.Resume();
        }
    }

    private readonly ISuspendableOut _regular;
    private readonly ISuspendableOut _error;

    public OutSuspender(
        ISuspendableOut regular,
        ISuspendableOut error)
    {
        _regular = regular;
        _error = error;
    }

    public IDisposable Suspend()
        => new Suspender(_regular, _error);
}