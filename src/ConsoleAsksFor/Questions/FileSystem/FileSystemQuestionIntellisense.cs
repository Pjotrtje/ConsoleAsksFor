namespace ConsoleAsksFor;

internal sealed class FileSystemQuestionIntellisense : IIntellisense
{
    private readonly bool _includeFiles;
    private readonly IReadOnlySet<string>? _allowedExtensions;

    public FileSystemQuestionIntellisense(bool includeFiles, IReadOnlySet<string>? allowedExtensions)
    {
        _includeFiles = includeFiles;
        _allowedExtensions = allowedExtensions;
    }

    public string? CompleteValue(string value)
        => Handle(value, value, IntellisenseDirection.None);

    public string? PreviousValue(string value, string hint)
        => Handle(value, hint, IntellisenseDirection.Previous);

    public string? NextValue(string value, string hint)
        => Handle(value, hint, IntellisenseDirection.Next);

    private string? Handle(string value, string hint, IntellisenseDirection direction)
    {
        var fixedHint = GetFixedHint(value, hint);

        var directory = DriveExists(fixedHint)
            ? fixedHint
            : Path.GetDirectoryName(fixedHint) ?? string.Empty;

        if (!Directory.Exists(directory) && !DriveExists(directory))
        {
            return null;
        }

        var files = _includeFiles
            ? Directory.GetFiles(directory).Where(f => _allowedExtensions is null || _allowedExtensions.Contains(Path.GetExtension(f)))
            : [];

        var directories = Directory.GetDirectories(directory);

        var subItems = files.Concat(directories)
            .OrderBy(x => x)
            .Where(e => e.StartsWith(fixedHint, true, CultureInfo.InvariantCulture))
            .ToList();

        if (!subItems.Any())
        {
            return null;
        }

        int GetCurrentIndex()
            => subItems.FindIndex(x => x.Equals(value, StringComparison.OrdinalIgnoreCase));

        var toSelectIndex = direction switch
        {
            IntellisenseDirection.Previous => GetCurrentIndex() - 1,
            IntellisenseDirection.Next => GetCurrentIndex() + 1,
            _ => 0,
        };

        string? GetIfChanged(string newValue)
        {
            var fixedValue = PathCapitalizationFixer.Fix(newValue);
            return fixedValue == value
                ? null
                : fixedValue;
        }

        return toSelectIndex switch
        {
            < 0 => GetIfChanged(subItems.Last()),
            _ when toSelectIndex >= subItems.Count => GetIfChanged(subItems.First()),
            _ => GetIfChanged(subItems[toSelectIndex]),
        };
    }

    private static string GetFixedHint(string value, string hint)
    {
        if (hint != "")
        {
            return hint;
        }

        if (value.EndsWith("\\", StringComparison.InvariantCultureIgnoreCase))
        {
            return value;
        }

        if (File.Exists(value) || Directory.Exists(value))
        {
            var path = Path.GetDirectoryName(value)!;
            return DriveExists(path)
                ? path
                : path + "\\";
        }
        return value;
    }

    private static bool DriveExists(string path)
    {
        var drive = Path.GetPathRoot(path);
        return drive == path && Directory.Exists(drive);
    }
}
