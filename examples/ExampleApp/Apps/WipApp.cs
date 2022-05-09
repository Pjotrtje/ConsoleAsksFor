using System.Collections.Generic;

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

        _console.WriteSplitter(ConsoleColor.Blue);
        _console.WriteCustomLine("sdf", ConsoleColor.DarkCyan);
        var xx = await _console.AskForItems("", new List<string> { "", "A", "B" });
        await _console.AskForLocalTime("How late?", Between(new LocalTime(11, 01, 00, 01), new LocalTime(11, 02, 00, 02)));

        await _console.AskForTimeOnly("When?", Between(new TimeOnly(11, 01, 00, 01), new TimeOnly(11, 01, 00, 02)));
    }
}
