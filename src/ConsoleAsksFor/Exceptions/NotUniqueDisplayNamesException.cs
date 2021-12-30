namespace ConsoleAsksFor;

/// <summary>
/// An exception that represents all the not unique display names.
/// </summary>
public sealed class NotUniqueDisplayNamesException : Exception
{
    /// <summary>
    /// Details for not unique display name.
    /// </summary>
    public sealed record NotUniqueDisplayName(string Name, IReadOnlyCollection<int> Indexes)
    {
        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            var indexes = Indexes.JoinAsStrings(", ");
            return $"Name: '{Name}', at indexes: [{indexes}]";
        }
    }

    /// <summary>
    /// All the not unique display names.
    /// </summary>
    public IReadOnlyCollection<NotUniqueDisplayName> NotUniqueDisplayNames { get; }

    internal NotUniqueDisplayNamesException(IReadOnlyCollection<NotUniqueDisplayName> notUniqueDisplayNames)
        : base(ToMessage(notUniqueDisplayNames))
        => NotUniqueDisplayNames = notUniqueDisplayNames;

    private static string ToMessage(IEnumerable<NotUniqueDisplayName> notUniqueDisplayNames)
    {
        var caption = "Non unique displayname(s) found:";
        var notUniques = notUniqueDisplayNames.JoinAsStrings(Environment.NewLine);
        return $"{caption}{Environment.NewLine}{notUniques}";
    }
}