namespace ExampleApp.Apps;

internal sealed class DemoApp
{
    private readonly IConsole _console;

    public DemoApp(IConsole console)
    {
        _console = console;
    }

    public async Task Run()
    {
        var console = _console;
        console.WriteInfoLine("Tip: Use arrows to go through history.");
        var wordOfTheDay = await console.AskForString("What is your word of the day?");

        console.WriteInfoLine("Tip: Use tab for intellisense.");
        var likableWords = await console.AskForItems("Which of these words do you like?", new[] { "Whale", "Yesterday", "Some", "Stereo", "Random" });

        await Task.Delay(TimeSpan.FromSeconds(1));
    }
}