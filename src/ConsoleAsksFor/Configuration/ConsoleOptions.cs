namespace ConsoleAsksFor;

/// <summary>
/// Options for ConsoleAsksFor.
/// </summary>
public sealed record ConsoleOptions
{
    /// <summary>
    /// Default value: <see cref="ConsoleColors.Default" />.
    /// </summary>
    public ConsoleColors Colors { get; init; } = ConsoleColors.Default;

    /// <summary>
    /// Default value: <see cref="HistoryOptions.Default" />.
    /// </summary>
    public HistoryOptions History { get; init; } = HistoryOptions.Default;

    /// <summary>
    /// Default value: <see cref="LoggingOptions.Default" />.
    /// </summary>
    public LoggingOptions Logging { get; init; } = LoggingOptions.Default;

    /// <summary>
    /// When asking question when input is idle for this amount of seconds all Console.Out in queue is flushed and printed to screen. <br/>
    /// This can theoretically result in a minimal hick-up. So setting to 1 seconds is not optimal. <br/>
    /// When value is lower than 1, 1 is still used.
    /// Default value: 100.
    /// </summary>
    public int OnIdleKeyPressFlushOutEverySeconds { get; init; } = 100;

    private ConsoleOptions()
    {
    }

    /// <summary>
    /// Default <see cref="ConsoleOptions"/>.
    /// </summary>
    public static ConsoleOptions Default { get; } = new();
}
