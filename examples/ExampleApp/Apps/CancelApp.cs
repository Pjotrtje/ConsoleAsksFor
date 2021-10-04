using System;
using System.Threading;
using System.Threading.Tasks;

using ConsoleAsksFor;

namespace ExampleApp.Apps
{
    internal sealed class CancelApp : IApp
    {
        private readonly IConsole _console;

        public CancelApp(IConsole console)
        {
            _console = console;
        }

        public async Task Run()
        {
            _console.WriteHelpTextLines();

            await DoWithTryCatch();
            await DoWithNullableWhenF12();
        }

        private async Task DoWithTryCatch()
        {
            const int seconds = 10;
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(seconds));

            try
            {
                var value = await _console.AskForBool($"Another bool question (you have {seconds} seconds to answer)?", cancellationToken: cts.Token);
                _console.WriteSuccessLine($"{value}");
            }
            catch (TaskCanceledByF12Exception)
            {
                _console.WriteErrorLine("You pushed F12.");
            }
            catch (TaskCanceledException)
            {
                _console.WriteErrorLine("Sorry, you waited too long.");
            }
        }

        private async Task DoWithNullableWhenF12()
        {
            const int seconds = 10;
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(seconds));

            try
            {
                var value = await _console.AskForBool($"Another bool question (you have {seconds} seconds to answer)?", cancellationToken: cts.Token).WithNullableWhenF12();

                if (value is null)
                {
                    _console.WriteErrorLine("You pushed F12.");
                }
                else
                {
                    _console.WriteSuccessLine($"{value}");
                }
            }
            catch (TaskCanceledException)
            {
                _console.WriteErrorLine("Sorry, you waited too long.");
            }
        }
    }
}