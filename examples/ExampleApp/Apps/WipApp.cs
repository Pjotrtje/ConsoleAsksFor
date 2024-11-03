using System.Linq;

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

    private sealed record Uzovi(string Value) : IParsable<Uzovi>, IFormattable
    {
        public static Uzovi Parse(string s, IFormatProvider? provider)
            => TryParse(s, provider, out var result)
                ? result
                : throw new ArgumentException("Invalid", nameof(s));

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Uzovi result)
        {
            if (s is null)
            {
                result = null;
                return false;
            }

            if (s.All(char.IsNumber) && s.Length == 4)
            {
                result = new Uzovi(s);
                return true;
            }

            result = null;
            return false;
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
            => Value;

        public override string ToString()
            => ToString(null, null);
    }

    public async Task Run()
    {
        //var nowTimeOnly = TimeOnly.FromDateTime(DateTime.Now);
        //await _console.AskForTimeOnly("How late?", Between(TimeOnly.MinValue, nowTimeOnly.AddHours(1)), defaultValue: nowTimeOnly);

        //var nowLocalTime = LocalTime.Noon;
        //await _console.AskForLocalTime("How late?", Between(LocalTime.MinValue, nowLocalTime.PlusHours(1)), defaultValue: nowLocalTime);
        //await _console.AskForDirectory("Root", defaultValue: new DirectoryInfo("C:\\"));
        //await _console.AskForDirectory("Not Exist", defaultValue: new DirectoryInfo("C:\\temp\\b"));
        //await _console.AskForDirectory("Exist", defaultValue: new DirectoryInfo("C:\\temp\\BlazorApp"));

        await _console.AskForItems("test", [1, 2, 3, 100000]);

        await _console.AskForStringBasedValueObject<decimal>("Numbers", "Some");

        await _console.AskForStringBasedValueObject<Agb>("Agb", Agb.TryParse, x => x.ToString(), "8 numbers");
        await _console.AskForStringBasedValueObject<Uzovi>("Uzovi", "4 numbers");

        while (true)
        {
            foreach (var @enum in Enum.GetValues<TimeSpanUnitType>())
            {
                var ts = await _console.AskForTimeSpan("WHAT?!" + @enum.ToString(), @enum, Between(TimeSpan.MinValue, TimeSpan.MaxValue), TimeSpan.MaxValue);
                _console.WriteAnswerLine($"{ts}");
            }
        }

        //await _console.AskForExistingFileName("aa", defaultValue: new FileInfo("C:\\temp\\"));
        //await _console.AskForExistingFileName("aa", ".svg", new FileInfo("C:\\temp\\"));
        //await _console.AskForExistingFileName("aa", new[] { ".svg", ".log" }, new FileInfo("C:\\temp\\"));

        //await _console.AskForDirectory("aa", defaultValue: new DirectoryInfo("C:\\temp\\BlazorApp"));
        //await _console.AskForDirectory("aa", defaultValue: new DirectoryInfo("C:\\temp\\"));
        //await _console.AskForDirectory("aa", defaultValue: new DirectoryInfo("C:\\temp\\"));
        //await _console.AskForDirectory("aa", defaultValue: new DirectoryInfo("C:\\temp\\"));
        //await _console.AskForDirectory("aa", defaultValue: new DirectoryInfo("C:\\temp\\"));
        //await _console.AskForDirectory("aa", defaultValue: new DirectoryInfo("C:\\temp\\"));

        //await _console.AskForDirectory("aa", defaultValue: new DirectoryInfo("C:\\temp\\"));
        //await _console.AskForExistingFileName("aa", ".svg", new FileInfo("C:\\temp\\"));
        //await _console.AskForExistingFileName("aa", defaultValue: new FileInfo("C:\\temp\\"));
        //await _console.AskForLocalTime("How late?", Between(new LocalTime(11, 01, 00, 01), new LocalTime(11, 02, 00, 02)));

        //await _console.AskForTimeOnly("When?", Between(new TimeOnly(11, 01, 00, 01), new TimeOnly(11, 01, 00, 02)));
    }
}
