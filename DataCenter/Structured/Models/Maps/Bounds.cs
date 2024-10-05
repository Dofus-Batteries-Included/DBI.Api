namespace DBI.DataCenter.Structured.Models.Maps;

/// <summary>
///     2D bounds.
/// </summary>
public class Bounds
{
    /// <summary>
    ///     The position of the top left corner of the bounds.
    /// </summary>
    public Position Position { get; init; }

    /// <summary>
    ///     The size of the bounds.
    /// </summary>
    public Size Size { get; init; }
}
