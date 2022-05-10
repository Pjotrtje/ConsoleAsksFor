using System.Linq;

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

    private sealed record Agb(string Value)
    {
        public static bool TryParse(string value, [MaybeNullWhen(false)] out Agb agb)
        {
            if (value.All(char.IsNumber) && value.Length == 8)
            {
                agb = new Agb(value);
                return true;
            }

            agb = null;
            return false;
        }
    }

    public async Task Run()
    {
        //var nowTimeOnly = TimeOnly.FromDateTime(DateTime.Now);
        //await _console.AskForTimeOnly("How late?", Between(TimeOnly.MinValue, nowTimeOnly.AddHours(1)), defaultValue: nowTimeOnly);

        //var nowLocalTime = LocalTime.Noon;
        //await _console.AskForLocalTime("How late?", Between(LocalTime.MinValue, nowLocalTime.PlusHours(1)), defaultValue: nowLocalTime);

        await _console.AskForExistingFileName("aa", defaultValue: new FileInfo("C:\\temp\\"));
        await _console.AskForExistingFileName("aa", ".svg", new FileInfo("C:\\temp\\"));
        await _console.AskForExistingFileName("aa", new[] { ".svg", ".log" }, new FileInfo("C:\\temp\\"));

        await _console.AskForDirectory("aa", defaultValue: new DirectoryInfo("C:\\temp\\BlazorApp"));
        await _console.AskForDirectory("aa", defaultValue: new DirectoryInfo("C:\\temp\\"));
        await _console.AskForDirectory("aa", defaultValue: new DirectoryInfo("C:\\temp\\"));
        await _console.AskForDirectory("aa", defaultValue: new DirectoryInfo("C:\\temp\\"));
        await _console.AskForDirectory("aa", defaultValue: new DirectoryInfo("C:\\temp\\"));
        await _console.AskForDirectory("aa", defaultValue: new DirectoryInfo("C:\\temp\\"));

        await _console.AskForDirectory("aa", defaultValue: new DirectoryInfo("C:\\temp\\"));
        await _console.AskForExistingFileName("aa", ".svg", new FileInfo("C:\\temp\\"));
        await _console.AskForExistingFileName("aa", defaultValue: new FileInfo("C:\\temp\\"));
        await _console.AskForStringBasedValueObject<Agb>("Agb", Agb.TryParse, x => x.ToString(), "8 numbers");
        await _console.AskForLocalTime("How late?", Between(new LocalTime(11, 01, 00, 01), new LocalTime(11, 02, 00, 02)));

        await _console.AskForTimeOnly("When?", Between(new TimeOnly(11, 01, 00, 01), new TimeOnly(11, 01, 00, 02)));
    }
}
