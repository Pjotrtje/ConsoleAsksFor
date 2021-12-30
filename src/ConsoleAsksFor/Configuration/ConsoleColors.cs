namespace ConsoleAsksFor;

/// <summary>
/// Colors used in <see cref="IConsole"/>.
/// </summary>
public sealed record ConsoleColors
{
    /// <summary>
    /// Default value: Foreground=<see cref="ConsoleColor.Yellow" />, Background=<see cref="ConsoleColor.Black" />.
    /// </summary>
    public LineColor Logo { get; init; } = new(ConsoleColor.Yellow, ConsoleColor.Black);

    /// <summary>
    /// Default value: Foreground=<see cref="ConsoleColor.Red" />, Background=<see cref="ConsoleColor.Black" />.
    /// </summary>
    public LineColor Error { get; init; } = new(ConsoleColor.Red, ConsoleColor.Black);

    /// <summary>
    /// Default value: Foreground=<see cref="ConsoleColor.DarkYellow" />, Background=<see cref="ConsoleColor.Black" />.
    /// </summary>
    public LineColor Warning { get; init; } = new(ConsoleColor.DarkYellow, ConsoleColor.Black);

    /// <summary>
    /// Default value: Foreground=<see cref="ConsoleColor.Gray" />, Background=<see cref="ConsoleColor.Black" />.
    /// </summary>
    public LineColor Info { get; init; } = new(ConsoleColor.Gray, ConsoleColor.Black);

    /// <summary>
    /// Default value: Foreground=<see cref="ConsoleColor.Green" />, Background=<see cref="ConsoleColor.Black" />.
    /// </summary>
    public LineColor Success { get; init; } = new(ConsoleColor.Green, ConsoleColor.Black);

    /// <summary>
    /// Default value: Foreground=<see cref="ConsoleColor.Yellow" />, Background=<see cref="ConsoleColor.Black" />.
    /// </summary>
    public LineColor Question { get; init; } = new(ConsoleColor.Yellow, ConsoleColor.Black);

    /// <summary>
    /// Default value: Foreground=<see cref="ConsoleColor.Gray" />, Background=<see cref="ConsoleColor.Black" />.
    /// </summary>
    public LineColor QuestionHint { get; init; } = new(ConsoleColor.Gray, ConsoleColor.Black);

    /// <summary>
    /// Default value: Foreground=<see cref="ConsoleColor.White" />, Background=<see cref="ConsoleColor.Black" />.
    /// </summary>
    public LineColor Answer { get; init; } = new(ConsoleColor.White, ConsoleColor.Black);

    /// <summary>
    /// Default value: Foreground=<see cref="ConsoleColor.DarkYellow" />, Background=<see cref="ConsoleColor.Black" />.
    /// </summary>
    public LineColor InvalidAnswer { get; init; } = new(ConsoleColor.DarkYellow, ConsoleColor.Black);

    /// <summary>
    /// Default value: Foreground=<see cref="ConsoleColor.Magenta" />, Background=<see cref="ConsoleColor.Black" />.
    /// </summary>
    public LineColor HelpText { get; init; } = new(ConsoleColor.Magenta, ConsoleColor.Black);

    private ConsoleColors()
    {
    }

    /// <summary>
    /// Default <see cref="ConsoleColors"/>.
    /// </summary>
    public static ConsoleColors Default { get; } = new();
}