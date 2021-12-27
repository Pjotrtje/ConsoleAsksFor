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
        await _console.AskForItems("X1", new[] { "A", "a", "C" });
    }
}