namespace ConsoleAsksFor;

/// <summary>
/// Options related to logging.
/// </summary>
public sealed record LoggingOptions
{
    /// <summary>
    /// '.console\logs'
    /// </summary>
    internal const string DirectoryPath = @".console\logs";

    /// <summary>
    /// Whether or not log written items in <see cref="IConsole"/>. <br/>
    /// When enabled; File located in <inheritdoc cref="DirectoryPath"/>
    /// with file format 'yyyy-MM-dd_HH.mm.ss.log' (starting datetime of console app).<br/>
    /// Default value: true.
    /// </summary>
    public bool HasLog { get; init; } = true;

    /// <summary>
    /// LineTypes which should be logged when <see cref="HasLog"/>=true. <br/>
    /// Default value: { <see cref="LineTypeId.Question"/>, <see cref="LineTypeId.Answer"/> }.
    /// </summary>
    public IReadOnlyCollection<LineTypeId> ToLogLineTypes { get; init; } = [LineTypeId.Question, LineTypeId.Answer];

    /// <summary>
    /// Default <see cref="LoggingOptions"/>.
    /// </summary>
    public static LoggingOptions Default { get; } = new();

    /// <summary>
    /// Disabled logging.
    /// </summary>
    public static LoggingOptions None { get; } = new()
    {
        HasLog = false,
        ToLogLineTypes = Array.Empty<LineTypeId>(),
    };
}