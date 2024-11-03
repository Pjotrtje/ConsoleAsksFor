namespace ConsoleAsksFor;

internal sealed class DirectoryQuestion : IQuestion<DirectoryInfo>
{
    public string SubType => $"{_fileSystemExistence}";

    public bool MustObfuscateAnswer => false;

    public IIntellisense Intellisense { get; } = new FileSystemQuestionIntellisense(false, null);

    public string Text { get; }

    public string PrefilledValue => _defaultValue?.FullName ?? string.Empty;

    private readonly FileSystemExistence _fileSystemExistence;
    private readonly DirectoryInfo? _defaultValue;

    public DirectoryQuestion(
        string text,
        FileSystemExistence fileSystemExistence,
        DirectoryInfo? defaultValue)
    {
        Text = text;
        _fileSystemExistence = fileSystemExistence;
        _defaultValue = defaultValue;
    }

    public IEnumerable<string> GetHints()
        => [];

    public bool TryParse(
        string answerAsString,
        [MaybeNullWhen(true)] out IEnumerable<string> errors,
        [MaybeNullWhen(false)] out DirectoryInfo answer)
    {
        var path = answerAsString;
        if (!IsValidDirectory(path))
        {
            errors = ["Not a valid directory."];
            answer = default;
            return false;
        }

        if (_fileSystemExistence == FileSystemExistence.New && Directory.Exists(path))
        {
            errors = ["Directory already exists."];
            answer = default;
            return false;
        }

        if (File.Exists(path))
        {
            errors = ["Expected directory but found file."];
            answer = default;
            return false;
        }

        if (_fileSystemExistence == FileSystemExistence.Existing && !Directory.Exists(path))
        {
            errors = ["Directory does not exists."];
            answer = default;
            return false;
        }

        errors = [];
        answer = new DirectoryInfo(path);
        return true;
    }

    public string FormatAnswer(DirectoryInfo answer)
        => PathCapitalizationFixer.Fix(answer);

    private static bool IsValidDirectory(string path)
    {
        var invalidPathChars = Path.GetInvalidPathChars();
        return !string.IsNullOrWhiteSpace(path) &&
               !invalidPathChars.Any(path.Contains) &&
               DriveOfDirectoryExists(path);
    }

    private static bool DriveOfDirectoryExists(string path)
    {
        var drive = Path.GetPathRoot(path);
        return Directory.Exists(drive);
    }
}
