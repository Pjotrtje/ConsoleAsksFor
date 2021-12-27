using System;
using System.Threading;
using System.Threading.Tasks;

using ConsoleAsksFor;

using Microsoft.Extensions.Logging;

namespace ExampleApp.Apps
{
    internal sealed class LoggingApp : IApp
    {
        private readonly IConsole _console;
        private readonly ILogger<LoggingApp> _logger;

        public LoggingApp(
            IConsole console,
            ILogger<LoggingApp> logger)
        {
            _console = console;
            _logger = logger;
        }

        public async Task Run()
        {
            using var cts = new CancellationTokenSource();

            try
            {
                FireAndForget.RepeatWithDelay(
                    TimeSpan.FromMilliseconds(2000),
                    () => _logger.LogInformation("LogInformation! {Time}", DateTime.Now),
                    cts.Token);

                FireAndForget.RepeatWithDelay(
                    TimeSpan.FromMilliseconds(1000),
                    () => _logger.LogError("LogError! {Time}", DateTime.Now),
                    cts.Token);

                while (await _console.AskForBool("Another question?", cancellationToken: cts.Token))
                {
                }
            }
            finally
            {
                cts.Cancel();
            }
        }
    }
}