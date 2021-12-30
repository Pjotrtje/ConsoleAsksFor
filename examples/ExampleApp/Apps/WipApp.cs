using ConsoleAsksFor.NodaTime.ISO;

using NodaTime;

namespace ExampleApp.Apps;

internal sealed class WipApp : IApp
{
    private readonly IConsole _console;

    public WipApp(IConsole console)
    {
        _console = console;
    }

    public async Task Run()
    {
        var nowTimeOnly = TimeOnly.FromDateTime(DateTime.Now);
        await _console.AskForTimeOnly("How late?", Between(TimeOnly.MinValue, nowTimeOnly.AddHours(1)), defaultValue: nowTimeOnly);

        var nowLocalTime = LocalTime.Noon;
        await _console.AskForLocalTime("How late?", Between(LocalTime.MinValue, nowLocalTime.PlusHours(1)), defaultValue: nowLocalTime);
    }
}