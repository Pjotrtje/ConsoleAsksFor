namespace ConsoleAsksFor
{
    /// <summary>
    /// Options related to history.
    /// </summary>
    public sealed record HistoryOptions
    {
        /// <summary>
        /// '.console\history.json'
        /// </summary>
        internal const string FilePath = @".console\history.json";

        /// <summary>
        /// Whether or not history is persisted between sessions. <br/>
        /// When enabled; file located at <inheritdoc cref="FilePath"/>.<br/>
        /// Default value: true.
        /// </summary>
        public bool HasPersistedHistory { get; init; } = true;

        /// <summary>
        /// Amount of answers of questions which are stored in history. <br/>
        /// Before an answer can be given history is determined vor question. If you notice delays lowering this value could be useful. <br/>
        /// After each given answer data is persisted to file. If you notice delays lowering this value could be useful. <br/>
        /// Default value: <see cref="int.MaxValue" />.
        /// </summary>
        public int MaxSize { get; init; } = int.MaxValue;

        /// <summary>
        /// Default <see cref="HistoryOptions"/>.
        /// </summary>
        public static HistoryOptions Default { get; } = new();

        /// <summary>
        /// Disabled persistence of history.
        /// </summary>
        public static HistoryOptions NoPersistence { get; } = new()
        {
            HasPersistedHistory = false,
        };
    }
}