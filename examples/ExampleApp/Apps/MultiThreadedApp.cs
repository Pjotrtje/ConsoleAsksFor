using System;
using System.Threading;
using System.Threading.Tasks;

using ConsoleAsksFor;

namespace ExampleApp.Apps;

internal sealed class MultiThreadedApp : IApp
{
    private readonly IConsole _console;

    public MultiThreadedApp(IConsole console)
    {
        _console = console;
    }

    public async Task Run()
    {
        const int seconds = 10;
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(seconds));
        var cancellationToken = cts.Token;

        try
        {
            FireAndForget.RepeatWithDelay(
                TimeSpan.FromMilliseconds(1),
                async () => _ = await _console.AskForBool("Are you sure from thread1?", cancellationToken: cancellationToken),
                cancellationToken);

            FireAndForget.RepeatWithDelay(
                TimeSpan.FromMilliseconds(1),
                async () => _ = await _console.AskForBool("Are you sure from thread2?", cancellationToken: cancellationToken),
                cancellationToken);

            _console.WriteWarningLine($"App will sleep for {seconds} seconds.");
            await Task.Delay(TimeSpan.FromSeconds(seconds), cancellationToken);
            _console.WriteErrorLine("Timeout!");
        }
        finally
        {
            cts.Cancel();
        }
    }
}