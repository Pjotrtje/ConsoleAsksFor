namespace ConsoleAsksFor;

/// <summary>
/// Parse string to TValueType
/// </summary>
/// <typeparam name="TValueType"></typeparam>
public delegate bool TryParse<TValueType>(string str, [MaybeNullWhen(false)] out TValueType valueType);
