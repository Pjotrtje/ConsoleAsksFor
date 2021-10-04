using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExampleApp
{
    internal static class FireAndForget
    {
        public static void RepeatWithDelay(TimeSpan delay, Func<Task> action, CancellationToken cancellationToken)
        {
            Repeat(async () =>
                {
                    await Task.Delay(delay, cancellationToken);
                    await action();
                },
                cancellationToken);
        }

        public static void RepeatWithDelay(TimeSpan delay, Action action, CancellationToken cancellationToken)
        {
            Repeat(async () =>
                {
                    await Task.Delay(delay, cancellationToken);
                    action();
                },
                cancellationToken);
        }

        public static void Repeat(Func<Task> action, CancellationToken cancellationToken)
        {
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await action();
                }
            }, cancellationToken);
        }
    }
}