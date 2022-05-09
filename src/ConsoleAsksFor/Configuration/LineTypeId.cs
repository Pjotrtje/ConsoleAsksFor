namespace ConsoleAsksFor;

/// <summary>
/// Identifier for what kind of type line is.
/// </summary>
public enum LineTypeId
{
    /// <summary>
    /// Id for <see cref="Error"/>.
    /// </summary>
    Error = 1,

    /// <summary>
    /// Id for <see cref="Warning"/>.
    /// </summary>
    Warning = 2,

    /// <summary>
    /// Id for <see cref="Info"/>.
    /// </summary>
    Info = 4,

    /// <summary>
    /// Id for <see cref="Success"/>.
    /// </summary>
    Success = 8,

    /// <summary>
    /// Id for <see cref="Question"/>.
    /// </summary>
    Question = 16,

    /// <summary>
    /// Id for <see cref="QuestionHint"/>.
    /// </summary>
    QuestionHint = 32,

    /// <summary>
    /// Id for <see cref="Answer"/>.
    /// </summary>
    Answer = 64,

    /// <summary>
    /// Id for <see cref="InvalidAnswer"/>.
    /// </summary>
    InvalidAnswer = 128,

    /// <summary>
    /// Id for <see cref="HelpText"/>.
    /// </summary>
    HelpText = 256,

    /// <summary>
    /// Id for all other lines.
    /// </summary>
    Other = 512,
}
