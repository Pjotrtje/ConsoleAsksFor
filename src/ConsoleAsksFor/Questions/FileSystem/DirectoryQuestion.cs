namespace ConsoleAsksFor;

internal sealed class DirectoryQuestion : IQuestion<DirectoryInfo>
{
    public string SubType => $"{_fileSystemExistence}";

    public bool MustObfuscateAnswer => false;

    public IIntellisense Intellisense { get; } = new FileSystemQuestionIntellisense(false, string.Empty);

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
        => Enumerable.Empty<string>();

    public bool TryParse(
        string answerAsString,
        [MaybeNullWhen(true)] out IEnumerable<string> errors,
        [MaybeNullWhen(false)] out DirectoryInfo answer)
    {
        var path = answerAsString;
        if (!IsValidDirectory(path))
        {
            errors = new[] { "Not a valid directory." };
            answer = default;
            return false;
        }

        if (_fileSystemExistence == FileSystemExistence.New && Directory.Exists(path))
        {
            errors = new[] { "Directory already exists." };
            answer = default;
            return false;
        }

        if (File.Exists(path))
        {
            errors = new[] { "Expected directory but found file." };
            answer = default;
            return false;
        }

        if (_fileSystemExistence == FileSystemExistence.Existing && !Directory.Exists(path))
        {
            errors = new[] { "Directory does not exists." };
            answer = default;
            return false;
        }

        errors = Enumerable.Empty<string>();
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
