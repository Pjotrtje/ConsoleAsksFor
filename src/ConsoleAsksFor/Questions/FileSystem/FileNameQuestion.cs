namespace ConsoleAsksFor;

internal sealed class FileNameQuestion : IQuestion<FileInfo>
{
    public string SubType => $"{_fileSystemExistence}";

    public bool MustObfuscateAnswer => false;

    public IIntellisense Intellisense { get; }

    public string Text { get; }

    public string PrefilledValue => _defaultValue?.FullName ?? string.Empty;

    private readonly FileSystemExistence _fileSystemExistence;
    private readonly IReadOnlySet<string>? _allowedExtensions;
    private readonly FileInfo? _defaultValue;

    public FileNameQuestion(
        string text,
        FileSystemExistence fileSystemExistence,
        IEnumerable<string>? allowedExtensions,
        FileInfo? defaultValue)
    {
        Text = text;
        _fileSystemExistence = fileSystemExistence;
        _allowedExtensions = allowedExtensions?
            .Where(x => x.Length > 0)
            .Select(x => x[0] == '.' ? x : $".{x}")
            .ToHashSet();
        _defaultValue = defaultValue;
        Intellisense = new FileSystemQuestionIntellisense(true, _allowedExtensions);
    }

    public IEnumerable<string> GetHints()
        => _allowedExtensions is null
            ? Enumerable.Empty<string>()
            : new[] { $"Allowed extension(s): {_allowedExtensions.JoinStrings(", ")}" };

    public bool TryParse(
        string answerAsString,
        [MaybeNullWhen(true)] out IEnumerable<string> errors,
        [MaybeNullWhen(false)] out FileInfo answer)
    {
        var path = answerAsString;
        if (!IsValidFileName(path))
        {
            errors = new[] { "Not a valid filename." };
            answer = default;
            return false;
        }

        if (_fileSystemExistence == FileSystemExistence.New && File.Exists(path))
        {
            errors = new[] { "File already exists." };
            answer = default;
            return false;
        }

        if (Directory.Exists(path))
        {
            errors = new[] { "Expected file but found directory." };
            answer = default;
            return false;
        }

        if (_fileSystemExistence == FileSystemExistence.Existing && !File.Exists(path))
        {
            errors = new[] { "File does not exists." };
            answer = default;
            return false;
        }

        if (!Path.HasExtension(path))
        {
            errors = new[] { "Extension missing." };
            answer = default;
            return false;
        }

        if (_allowedExtensions is not null && !_allowedExtensions.Contains(Path.GetExtension(path)))
        {
            errors = new[] { "Extension not allowed." };
            answer = default;
            return false;
        }

        errors = Enumerable.Empty<string>();
        answer = new FileInfo(path);
        return true;
    }

    public string FormatAnswer(FileInfo answer)
        => PathCapitalizationFixer.Fix(answer);

    private static bool IsValidFileName(string path)
    {
        var invalidFileNameChars = Path.GetInvalidFileNameChars();
        var fileName = Path.GetFileName(path);
        return !string.IsNullOrWhiteSpace(fileName) &&
               !invalidFileNameChars.Any(fileName.Contains) &&
               ContainingDirectoryIsValid(path) &&
               DriveOfDirectoryExists(path);
    }

    private static bool ContainingDirectoryIsValid(string path)
    {
        var invalidPathChars = Path.GetInvalidPathChars();
        var directoryName = Path.GetDirectoryName(path);
        return !string.IsNullOrWhiteSpace(directoryName) &&
               !invalidPathChars.Any(directoryName.Contains);
    }

    private static bool DriveOfDirectoryExists(string path)
    {
        var drive = Path.GetPathRoot(path);
        return Directory.Exists(drive);
    }
}
