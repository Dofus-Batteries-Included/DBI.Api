namespace Server.Common.Models;

/// <summary>
///     A piece of text in all supported languages.
/// </summary>
public class LocalizedText
{
    /// <summary>
    ///     Text in french.
    /// </summary>
    public string? French { get; init; }

    /// <summary>
    ///     Text in english.
    /// </summary>
    public string? English { get; init; }

    /// <summary>
    ///     Text in spanish.
    /// </summary>
    public string? Spanish { get; init; }

    /// <summary>
    ///     Text in german.
    /// </summary>
    public string? German { get; init; }

    /// <summary>
    ///     Text in portuguese.
    /// </summary>
    public string? Portuguese { get; init; }
}
