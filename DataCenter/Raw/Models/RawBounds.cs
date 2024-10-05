namespace DBI.DataCenter.Raw.Models;

/// <summary>
///     2D bounds.
/// </summary>
public class RawBounds
{
    /// <summary>
    ///     The position of the top left corner of the bounds.
    /// </summary>
    public RawPosition Position { get; init; }

    /// <summary>
    ///     The size of the bounds.
    /// </summary>
    public RawSize Size { get; init; }
}
