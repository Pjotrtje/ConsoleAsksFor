using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAsksFor
{
    // https://gist.github.com/SHSE/5107198
    internal sealed class AsyncAutoResetEvent
    {
        private readonly ConcurrentQueue<TaskCompletionSource<bool>> _handlers = new();

        private int _isSet;

        public AsyncAutoResetEvent(bool initialState)
        {
            _isSet = initialState ? 1 : 0;
        }

        public void Set()
        {
            if (!TrySet())
            {
                return;
            }

            // Notify first alive handler
            while (_handlers.TryDequeue(out var handler))
            {
                if (CheckIfAlive(handler)) // Flag check
                {
                    lock (handler)
                    {
                        if (!CheckIfAlive(handler))
                        {
                            continue;
                        }

                        if (TryReset())
                        {
                            handler.SetResult(true);
                        }
                        else
                        {
                            _handlers.Enqueue(handler);
                        }

                        break;
                    }
                }
            }
        }

        private bool TrySet()
        {
            return Interlocked.CompareExchange(ref _isSet, 1, 0) == 0;
        }

        public Task WaitAsync(CancellationToken cancellationToken)
        {
            if (TryReset())
            {
                return Task.FromResult(true);
            }

            cancellationToken.ThrowIfCancellationRequested();

            // Wait for a signal
            var handler = new TaskCompletionSource<bool>(false);

            _handlers.Enqueue(handler);

            if (CheckIfAlive(handler)) // Flag check
            {
                lock (handler)
                {
                    if (CheckIfAlive(handler) && TryReset())
                    {
                        handler.SetResult(true);
                        return handler.Task;
                    }
                }
            }

            cancellationToken.Register(() =>
            {
                if (CheckIfAlive(handler)) // Flag check
                {
                    lock (handler)
                    {
                        if (CheckIfAlive(handler))
                        {
                            // Because I do not understand this code, I keep it as in gist...
                            // ReSharper disable once MethodSupportsCancellation
                            handler.SetCanceled();
                        }
                    }
                }
            });

            return handler.Task;
        }

        private static bool CheckIfAlive(TaskCompletionSource<bool> handler)
            => handler.Task.Status == TaskStatus.WaitingForActivation;

        private bool TryReset()
            => Interlocked.CompareExchange(ref _isSet, 0, 1) == 1;
    }
}