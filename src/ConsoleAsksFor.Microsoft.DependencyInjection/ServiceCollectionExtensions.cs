using System;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="IServiceCollection" /> to register <see cref="IConsole"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    private const string ObsoleteMessage = $"Use {nameof(ConsoleFactory)}.{nameof(ConsoleFactory.Create)} as your first line in your console app and register instance in your DI as Singleton";

    /// <summary>
    /// Adds <see cref="IConsole" /> with all it's dependencies to <see cref="IServiceCollection" /> with options <see cref="ConsoleOptions.Default" />.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    [Obsolete(ObsoleteMessage)]
    public static IServiceCollection AddConsoleAsksFor(this IServiceCollection services)
        => services.AddConsoleAsksFor(ConsoleOptions.Default);

    /// <summary>
    /// Adds <see cref="IConsole" /> with all it's dependencies to <see cref="IServiceCollection" /> with customized <see cref="ConsoleOptions" />.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options">Customized <see cref="ConsoleOptions" />.</param>
    /// <returns></returns>
    [Obsolete(ObsoleteMessage)]
    public static IServiceCollection AddConsoleAsksFor(this IServiceCollection services, ConsoleOptions options)
    {
        var console = ConsoleFactory.Create(options);
        if (services.ConsoleLoggerIsAlreadyAdded())
        {
            console.WriteWarningLine($"{nameof(AddConsoleAsksFor)} should be called before AddLogging/AddConsole. Otherwise logging could mess up reading input which could result in runtime exceptions.");
        }
        return services
            .AddSingleton(console);
    }

    private static bool ConsoleLoggerIsAlreadyAdded(this IServiceCollection services)
        => services.Any(x => x.ImplementationType?.FullName == "Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider");
}
