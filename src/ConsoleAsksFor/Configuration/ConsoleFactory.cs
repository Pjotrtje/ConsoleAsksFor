using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ConsoleAsksFor.Sdk;

using C = System.Console;

namespace ConsoleAsksFor;

/// <summary>
/// Factory for creating <see cref="IConsole"/>.
/// </summary>
public static class ConsoleFactory
{
    private static readonly object ConsoleLockObject = new();

    private static Console? existingConsole;
    private static ConsoleOptions? existingOptions;

    /// <summary>
    /// Creates <see cref="IConsole"/> with default options.
    /// </summary>
    /// <returns></returns>
    public static IConsole Create()
        => Create(ConsoleOptions.Default);

    /// <summary>
    /// Creates <see cref="IConsole"/> with options <see cref="ConsoleOptions.Default" />.
    /// </summary>
    /// <returns></returns>
    public static IConsole Create(ConsoleOptions options)
    {
        lock (ConsoleLockObject)
        {
            if (existingConsole is not null)
            {
                existingConsole.WriteWarningLine($"Console is singleton, please call {nameof(ConsoleFactory)}.{nameof(Create)} only once. The already created Console is returned.");
                if (options != existingOptions)
                {
                    existingConsole.WriteWarningLine("Already created Console options differ from provided options.");
                }
            }
            else
            {
                existingConsole = GetConsole(options);
                existingOptions = options;
            }

            return existingConsole;
        }
    }

    private static Console GetConsole(ConsoleOptions options)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            AnsiColorsInWindows.Enable();
        }

        var keyInputHandler = new KeyInputHandler();
        var filesystem = new FileSystem();
        var systemConsole = new SystemConsole();
        var dateTimeProvider = new DateTimeProvider();
        var lineTypes = new LineTypes(options.Colors);

        var regularDirectOut = new DirectOut(C.Out);
        var regularSuspendableOut = new SuspendableOut(systemConsole, regularDirectOut);
        var errorSuspendableOut = new SuspendableOut(systemConsole, new DirectOut(C.Error));

        IWriteLineLogger writeLineLogger = options.Logging.HasLog
            ? new WriteLineLogger(
                filesystem,
                regularSuspendableOut,
                dateTimeProvider,
                lineTypes,
                options.Logging,
                DateTime.Now)
            : new WriteLineLoggerSub();

        var consoleLineWriter = new ConsoleLineWriter(
            regularSuspendableOut,
            writeLineLogger,
            lineTypes);

        IHistoryRepository historyRepository = options.History.HasPersistedHistory
            ? new HistoryRepository(
                filesystem,
                consoleLineWriter,
                HistoryOptions.FilePath,
                options.History.MaxSize)
            : new HistoryRepositoryStub(options.History.MaxSize);

        C.SetOut(regularSuspendableOut);
        C.SetError(errorSuspendableOut);

        var outSuspender = new OutSuspender(
            regularSuspendableOut,
            errorSuspendableOut);

        var consoleInputGetter = new ConsoleInputGetter(
            outSuspender,
            systemConsole,
            regularDirectOut,
            lineTypes,
            Math.Max(options.OnIdleKeyPressFlushOutEverySeconds, 1));

        var questionerFactory = new QuestionerFactory(
            consoleLineWriter,
            consoleInputGetter,
            keyInputHandler,
            historyRepository);

        var console = new Console(
            consoleLineWriter,
            questionerFactory);

        regularDirectOut.WriteLogo(options.Colors.Logo);
        ExecuteOnStartupHooks();

        return console;
    }

    private static void ExecuteOnStartupHooks()
        => Assembly
            .GetEntryAssembly()?
            .GetReferencedAssemblies()
            .Where(x => x.FullName.StartsWith("ConsoleAsksFor.", StringComparison.InvariantCultureIgnoreCase))
            .Select(Assembly.Load)
            .SelectMany(a => a.DefinedTypes)
            .Where(t => t.GetInterfaces().Any(i => i == typeof(IOnStartupHook)))
            .Select(t => (IOnStartupHook)Activator.CreateInstance(t)!)
            .ToList()
            .ForEach(h => h.Initialize());

    private static void WriteLogo(this IDirectOut regularDirectOut, LineColor logoColor)
    {
        regularDirectOut.WriteLine(logoColor.Colorize("                   _____         _______      ______ "));
        regularDirectOut.WriteLine(logoColor.Colorize("                  |   __|       |   _   |    |   ___|"));
        regularDirectOut.WriteLine(logoColor.Colorize("                  |  |          |  |_|  |    |   ___|"));
        regularDirectOut.WriteLine(logoColor.Colorize("                  |  |__        |   _   |    |  |    "));
        regularDirectOut.WriteLine(logoColor.Colorize("With the help of: |_____|onsole |__| |__|sks |__|or. "));
        regularDirectOut.WriteLine(logoColor.Colorize("Press F1 for help during answering question."));
    }
}