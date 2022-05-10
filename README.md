# ConsoleAsksFor
![Icon](https://raw.githubusercontent.com/Pjotrtje/ConsoleAsksFor/main/docs/icon-128x128.png) 

ConsoleAsksFor is library for asking input in a console app easily.

![Azure DevOps builds (branch)](https://img.shields.io/azure-devops/build/Pjotrtje/PvS/20/main)


## Goal
The aim of ConsoleAsksFor is removing the hassle of retrieving/parsing/retrying input in a console app.


## Installation
You can download the latest release / pre-release NuGet packages from nuget:


 | Package: |  |  | 
 | :--- | --- | --- |
 | ConsoleAsksFor | [![ConsoleAsksFor on NuGet](https://img.shields.io/nuget/v/ConsoleAsksFor.svg)](https://www.nuget.org/packages/ConsoleAsksFor) | [![ConsoleAsksFor downloads on NuGet](https://img.shields.io/nuget/dt/ConsoleAsksFor.svg)](https://www.nuget.org/packages/ConsoleAsksFor) |
 | ConsoleAsksFor.Microsoft.DependencyInjection | [![ConsoleAsksFor.Microsoft.DependencyInjection on NuGet](https://img.shields.io/nuget/v/ConsoleAsksFor.Microsoft.DependencyInjection.svg)](https://www.nuget.org/packages/ConsoleAsksFor.Microsoft.DependencyInjection) | [![ConsoleAsksFor.Microsoft.DependencyInjection downloads on NuGet](https://img.shields.io/nuget/dt/ConsoleAsksFor.Microsoft.DependencyInjection.svg)](https://www.nuget.org/packages/ConsoleAsksFor.Microsoft.DependencyInjection) | 
 | ConsoleAsksFor.NodaTime.ISO | [![ConsoleAsksFor.NodaTime.ISO on NuGet](https://img.shields.io/nuget/v/ConsoleAsksFor.NodaTime.ISO.svg)](https://www.nuget.org/packages/ConsoleAsksFor.NodaTime.ISO) | [![ConsoleAsksFor.NodaTime.ISO downloads on NuGet](https://img.shields.io/nuget/dt/ConsoleAsksFor.NodaTime.ISO.svg)](https://www.nuget.org/packages/ConsoleAsksFor.NodaTime.ISO) |


## Setup

Register Console with ConsoleAsksFor.Microsoft.DependencyInjection:

```csharp
services.AddConsoleAsksFor();
```

Resolve a service which has dependency on IConsole of resolve service directly:

```csharp
var console = serviceProvider.GetRequiredService<IConsole>();
```

Or create with factory:

```csharp
ConsoleFactory.Create();
```


## Start using
Some examples:

```csharp
var isSure = await console.AskForBool("Are you sure?", cancellationToken: cancellationToken);
var age = await console.AskForInt("What is your age?", Between(0, 125));
var length = await console.AskForDecimal("What is your length (in meters)?", Scale.Two, Between(0m, 2.5m));
var appointmentStart = await console.AskForDateTimeOffset("How late should we meet?", TimeZoneInfo.Local, AtLeast(DateTimeOffset.Now), defaultValue: DateTimeOffset.Now.AddHours(1));
var favoriteColor = await console.AskForEnum<ConsoleColor>("What is your favorite color?");
var preferredName = await console.AskForItem("Which name do you prefer?", new[] { "Jantje", "Pietje" });
var directory = await console.AskForExistingDirectory("Where to store file?", defaultValue: new DirectoryInfo(@"C:\Temp"));
var name = await console.AskForString("What is your name?");
var zipcode = await console.AskForString("What is your Dutch zipcode?", new Regex("^[1-9][0-9]{3}[A-Z]{2}$"), "Format: '5555AA' where first digit is not a 0");
var agb = await console.AskForStringBasedValueObject<Agb>("What is your agb?", Agb.TryParse, x => x.ToString(), "8 numbers");
var pw = await console.AskForPassword("What is the secret code?");
```


## Demo
```csharp
var console = ConsoleFactory.Create();
console.WriteInfoLine("Tip: Use arrows to go through history.");
var wordOfTheDay = await console.AskForString("What is your word of the day?");

console.WriteInfoLine("Tip: Use tab for intellisense.");
var likableWords = await console.AskForItems("Which of these words do you like?", new[] { "Whale", "Yesterday", "Some", "Stereo", "Random" });
```
![Demo](https://raw.githubusercontent.com/Pjotrtje/ConsoleAsksFor/main/docs/demo.gif)


## Features
* Default values
* History
  * ByQuestionTextAndType (default)
  * ByQuestionType
  * NotFiltered
* Basic Intellisense
* Colors
  * Visual feedback whether current answer is correct
  * Different colors for different line types
  * Customizable
* Cancelable
  * Support for cancellationTokens
  * Push F12 to throw TaskCanceledByF12Exception
* Basic multithreading support:
  * During asking questions other output to Console is suspended
  * When multiple threads are asking questions, questions are queued


## Supported types
### ConsoleAsksFor
* AskForBool
* AskForByte 
* AskForSignedByte
* AskForShort 
* AskForUnsignedShort
* AskForInt 
* AskForUnsignedInt
* AskForLong 
* AskForUnsignedLong
* AskForDecimal
* AskForDateOnly
* AskForTimeOnly
* AskForDateTime
* AskForDateTimeOffset
* AskForTimeSpan
* AskForDirectory 
* AskForExistingDirectory 
* AskForNewDirectory
* AskForFileName 
* AskForExistingFileName 
* AskForNewFileName
* AskForEnum\<T>
* AskForFlaggedEnum\<T>
* AskForItem\<T>
* AskForItems\<T>
* AskForString
* AskForStringBasedValueObject\<T>
* AskForPassword


### ConsoleAsksFor.NodaTime.ISO
* AskForInstant
* AskForLocalDate
* AskForLocalDateTime
* AskForLocalTime
* AskForZonedDateTime
* AskForAnnualDate
* AskForYearMonth


## Release notes
See the [Releases page](https://github.com/Pjotrtje/ConsoleAsksFor/releases/).


## Versioning
ConsoleAsksFor follows [Semantic Versioning 2.0.0](http://semver.org/spec/v2.0.0.html) for the releases published to [nuget.org](https://www.nuget.org/).

## Credits
* [Sergey Shumov](https://gist.github.com/SHSE) for showing his [AsyncAutoResetEvent](https://gist.github.com/SHSE/5107198).
* [Jerrie Pelser](https://www.jerriepelser.com/) for showing how to [enable ansi colors in windows](https://www.jerriepelser.com/blog/using-ansi-color-codes-in-net-console-apps/).
* [JSQuareD](https://stackoverflow.com/users/1370541/jsquared) for showing how to [cancel Console.ReadKey](https://stackoverflow.com/questions/57615/how-to-add-a-timeout-to-console-readline/18342182#18342182).
* [Sergey](https://stackoverflow.com/users/1844247/sergey) for showing how to [lock resource in combination with async/await](https://stackoverflow.com/questions/7612602/why-cant-i-use-the-await-operator-within-the-body-of-a-lock-statement/50139704#50139704).
* [Asbjørn Ulsberg](https://gist.github.com/asbjornu) for showing how to [get amount of digits after decimal point](https://gist.github.com/asbjornu/c1f43647c9c2e3723a7a).

## Other
Another reason to make ConsoleAsksFor is because I have never worked with github and never created public nuget package. So this is a little brain exercise I guess :). 

