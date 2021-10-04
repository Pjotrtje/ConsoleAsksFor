using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using ConsoleAsksFor;

using static ConsoleAsksFor.RangeConstraint;

// ReSharper disable MethodSupportsCancellation

namespace ExampleApp.Apps
{
    internal sealed class ReadMeApp : IApp
    {
        private readonly IConsole _console;

        public ReadMeApp(IConsole console)
        {
            _console = console;
        }

        public async Task Run()
        {
            using var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            var console = _console;
            var isSure = await console.AskForBool("Are you sure?", cancellationToken: cancellationToken);
            var age = await console.AskForInt("What is your age?", Between(0, 125));
            var length = await console.AskForDecimal("What is your length (in meters)?", Scale.Two, Between(0m, 2.5m));
            var appointmentStart = await console.AskForDateTimeOffset("How late should we meet?", TimeZoneInfo.Local, AtLeast(DateTimeOffset.Now), defaultValue: DateTimeOffset.Now.AddHours(1));
            var favoriteColor = await console.AskForEnum<ConsoleColor>("What is your favorite color?");
            var preferredName = await console.AskForItem("Which name do you prefer?", new[] { "Jantje", "Pietje" });
            var directory = await console.AskForExistingDirectory("Where to store file?", defaultValue: new DirectoryInfo(@"C:\Temp"));
            var name = await console.AskForString("What is your name?");
            var zipcode = await console.AskForString("What is your Dutch zipcode?", new Regex("^[1-9][0-9]{3}[A-Z]{2}$"), "Format: '5555AA' where first digit is not a 0");
            var pw = await console.AskForPassword("What is the secret code?");
        }
    }
}