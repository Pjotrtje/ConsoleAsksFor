namespace ConsoleAsksFor;

internal sealed class HistoryRepository : IHistoryRepository
{
    private readonly IFileSystem _fileSystem;
    private readonly IConsoleLineWriter _consoleLineWriter;
    private readonly string _fileNamePath;
    private readonly int _maxSize;

    private bool _mustPersistHistory = true;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        WriteIndented = true,
    };

    public HistoryRepository(
        IFileSystem fileSystem,
        IConsoleLineWriter consoleLineWriter,
        string fileNamePath,
        int maxSize)
    {
        _fileSystem = fileSystem;
        _consoleLineWriter = consoleLineWriter;
        _fileNamePath = fileNamePath;
        _maxSize = maxSize;
    }

    public async Task<History> GetHistory()
    {
        try
        {
            var items = await GetHistoryItemsFromFile(_fileNamePath) ?? Array.Empty<HistoryItem>();
            return new History(items, _maxSize);
        }
        catch (Exception e)
        {
            _mustPersistHistory = false;
            _consoleLineWriter.WriteErrorLine(e.ToActionExceptionMessage(nameof(GetHistory)));
            _consoleLineWriter.WriteWarningLine("History is not fetched. New history will not be persisted.");
            return new History([], _maxSize);
        }
    }

    private async Task<IReadOnlyCollection<HistoryItem>?> GetHistoryItemsFromFile(string fileName)
    {
        if (!_fileSystem.FileExists(fileName))
        {
            return null;
        }

        var json = await _fileSystem.FileReadAllTextAsync(fileName);

        var items = JsonSerializer.Deserialize<HistoryItem[]>(json, JsonSerializerOptions);
        ThrowIfNotRespectingNullableReferenceTypes(items);
        return items;
    }

    private static void ThrowIfNotRespectingNullableReferenceTypes([NotNull] IEnumerable<HistoryItem>? historyItems)
    {
        if (historyItems is null || historyItems.Any(HasNullProperty))
        {
            throw new ArgumentException($"Cannot map file content (json) to {nameof(IReadOnlyCollection<HistoryItem>)}<{nameof(HistoryItem)}>");
        }
    }

    // ReSharper disable ConditionIsAlwaysTrueOrFalse, can be null because it is in content from file
    private static bool HasNullProperty(HistoryItem historyItem)
        => historyItem.QuestionText is null || historyItem.QuestionType is null || historyItem.Answer is null;

    // ReSharper restore ConditionIsAlwaysTrueOrFalse

    public async Task PersistHistory(History history)
    {
        if (!_mustPersistHistory)
        {
            return;
        }

        try
        {
            var directory = Path.GetDirectoryName(_fileNamePath)!;
            _fileSystem.CreateDirectory(directory);

            var json = JsonSerializer.Serialize(history.Items, JsonSerializerOptions);
            await _fileSystem.FileWriteAllTextAsync(_fileNamePath, json);
        }
        catch (Exception e)
        {
            _mustPersistHistory = false;
            _consoleLineWriter.WriteErrorLine(e.ToActionExceptionMessage(nameof(PersistHistory)));
            _consoleLineWriter.WriteWarningLine("History is not persisted. No more attempts are made during execution of this consoleapp.");
        }
    }
}