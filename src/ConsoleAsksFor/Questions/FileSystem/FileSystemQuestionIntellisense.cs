using System;
using System.Globalization;
using System.IO;
using System.Linq;

using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor;

internal sealed class FileSystemQuestionIntellisense : IIntellisense
{
    public string? CompleteValue(string value)
        => Handle(value, value, IntellisenseDirection.None);

    public string? PreviousValue(string value, string hint)
        => Handle(value, hint, IntellisenseDirection.Previous);

    public string? NextValue(string value, string hint)
        => Handle(value, hint, IntellisenseDirection.Next);

    private static string? Handle(string value, string hint, IntellisenseDirection direction)
    {
        var directory = DriveExists(hint)
            ? hint
            : Path.GetDirectoryName(hint) ?? string.Empty;

        if (!Directory.Exists(directory) && !DriveExists(directory))
        {
            return null;
        }

        var subItems = Directory
            .GetFileSystemEntries(directory)
            .OrderBy(x => x)
            .Where(e => e.StartsWith(hint, true, CultureInfo.InvariantCulture))
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

    private static bool DriveExists(string path)
    {
        var drive = Path.GetPathRoot(path);
        return drive == path && Directory.Exists(drive);
    }
}