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

        //var nowLocalTime = LocalTime.Noon;
        //await _console.AskForLocalTime("How late?", Between(nowLocalTime.PlusHours(1), nowLocalTime.PlusHours(-1)), defaultValue: nowLocalTime);

        await _console.AskForTimeOnly("When?", Between(new TimeOnly(11, 01, 00), new TimeOnly(03, 01, 00)));
    }
}