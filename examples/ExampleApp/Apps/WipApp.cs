using System;
using System.Threading.Tasks;

using ConsoleAsksFor;
using ConsoleAsksFor.NodaTime.ISO;

using NodaTime;

using static ConsoleAsksFor.RangeConstraint;

namespace ExampleApp.Apps
{
    internal sealed class WipApp : IApp
    {
        private readonly IConsole _console;

        public WipApp(IConsole console)
        {
            _console = console;
        }

        public async Task Run()
        {
            await _console.AskForAnnualDate("X1", AtLeast(new AnnualDate(12, 01)));
            await _console.AskForAnnualDate("X2");

            var hawaiianTime = TimeZoneInfo.FindSystemTimeZoneById("Hawaiian Standard Time");
            await _console.AskForDateTimeOffset("Met range", hawaiianTime, Between(DateTimeOffset.MinValue, DateTimeOffset.MinValue), DateTimeOffset.Now);
            await _console.AskForDateTimeOffset("Met range", hawaiianTime, Between(DateTimeOffset.MaxValue, DateTimeOffset.MaxValue), DateTimeOffset.UtcNow);
            await _console.AskForDateTimeOffset("Met range", TimeZoneInfo.Local, Between(DateTimeOffset.MinValue, DateTimeOffset.MaxValue));
            await _console.AskForDateTime("Met range", DateTimeKind.Utc, Between(DateTime.MinValue, DateTime.MaxValue));

            await _console.AskForDecimal("Zonder range", Scale.Two);
            await _console.AskForDecimal("Met range", Scale.Two, Between(decimal.MinValue, decimal.MaxValue));

            await _console.AskForDateTime("Zonder range", DateTimeKind.Utc);

            await _console.AskForDateTime("Met range + local", DateTimeKind.Local, Between(DateTime.MinValue, DateTime.MaxValue));

            var westEuropeStandardTime = DateTimeZoneProviders.Tzdb.GetZoneOrNull("Europe/Amsterdam")!;
            var defaultValue = westEuropeStandardTime.MapLocal(new LocalDateTime(2021, 10, 31, 02, 30)).Last();
            await _console.AskForZonedDateTime("Lala", westEuropeStandardTime, Between(defaultValue, defaultValue.Plus(Duration.FromDays(2))), defaultValue: defaultValue);
            await _console.AskForFlaggedEnum<Some>("Which name do you prefer?", Some.ABC);
        }

        [Flags]
        public enum Some
        {
            A = 1,
            B = 2,
            C = 4,
            D = 8,
            ABC = 7,
            E = 16,
        }
    }
}