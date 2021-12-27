namespace ConsoleAsksFor;

internal sealed class WriteLineLogger : IWriteLineLogger
{
    private readonly IFileSystem _fileSystem;
    private readonly ISuspendableOutWriter _suspendableOutWriter;
    private readonly IDateTimeProvider _dateTimeProvider;

    private readonly IReadOnlySet<LineTypeId> _toLogLineTypeIds;
    private readonly LineTypes _lineTypes;
    private readonly int _lineTypeStringMaxLength;

    private readonly AsyncLocker _fileLocker = new();

    private readonly string _fileNamePath;

    private bool _mustWriteLog = true;

    public WriteLineLogger(
        IFileSystem fileSystem,
        ISuspendableOutWriter suspendableOutWriter,
        IDateTimeProvider dateTimeProvider,
        LineTypes lineTypes,
        LoggingOptions options,
        DateTime startTime)
    {
        _fileSystem = fileSystem;
        _lineTypes = lineTypes;
        _suspendableOutWriter = suspendableOutWriter;
        _dateTimeProvider = dateTimeProvider;
        _lineTypeStringMaxLength = GetLineTypeStringMaxLength(options.ToLogLineTypes);
        _toLogLineTypeIds = options.ToLogLineTypes.ToHashSet();
        _fileNamePath = Path.Combine(LoggingOptions.DirectoryPath, $"{startTime:yyyy-MM-dd_HH.mm.ss}.log");
    }

    private static int GetLineTypeStringMaxLength(IEnumerable<LineTypeId> toLogLineTypes)
        => toLogLineTypes
            .Select(l => l.ToString())
            .Max(x => x.Length);

    public async Task LogToFile(LineTypeId lineTypeId, string value)
    {
        if (!_mustWriteLog || !_toLogLineTypeIds.Contains(lineTypeId))
        {
            return;
        }

        var lines = GetPrefixedLines(lineTypeId, value);

        try
        {
            var directory = Path.GetDirectoryName(_fileNamePath)!;
            await _fileLocker.LockAsync(async () =>
            {
                _fileSystem.CreateDirectory(directory);
                await _fileSystem.FileAppendAllLinesAsync(_fileNamePath, lines);
            });
        }
        catch (Exception e)
        {
            _mustWriteLog = false;

            var errorMessage = e.ToActionExceptionMessage(nameof(LogToFile));
            var colorizedErrorMessage = _lineTypes.Error.Color.Colorize(errorMessage);
            _suspendableOutWriter.WriteLine(colorizedErrorMessage);

            var warningMessage = "Console output is not logged. New console output will not be logged.";
            var colorizedWarningMessage = _lineTypes.Warning.Color.Colorize(warningMessage);
            _suspendableOutWriter.WriteLine(colorizedWarningMessage);
        }
    }

    private IEnumerable<string> GetPrefixedLines(LineTypeId lineTypeId, string value)
    {
        var dateAsString = $"{_dateTimeProvider.Now:yyyy-MM-dd HH:mm:ss}";
        var lineTypeAsString = lineTypeId.ToString().PadRight(_lineTypeStringMaxLength);
        return value
            .Split(Environment.NewLine)
            .Select(l => $"{dateAsString} | {lineTypeAsString} | {l}");
    }
}