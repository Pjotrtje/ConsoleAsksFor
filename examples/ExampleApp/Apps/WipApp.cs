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
        //var nowTimeOnly = TimeOnly.FromDateTime(DateTime.Now);
        //await _console.AskForTimeOnly("How late?", Between(TimeOnly.MinValue, nowTimeOnly.AddHours(1)), defaultValue: nowTimeOnly);

        //var nowLocalTime = LocalTime.Noon;
        //await _console.AskForLocalTime("How late?", Between(LocalTime.MinValue, nowLocalTime.PlusHours(1)), defaultValue: nowLocalTime);

        await _console.AskForLocalTime("How late?", Between(new LocalTime(11, 01, 00, 01), new LocalTime(11, 02, 00, 02)));

#if NET6_0_OR_GREATER
        await _console.AskForTimeOnly("When?", Between(new TimeOnly(11, 01, 00, 01), new TimeOnly(11, 01, 00, 02)));
#endif
    }
}