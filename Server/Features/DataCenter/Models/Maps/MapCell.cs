namespace Server.Features.DataCenter.Models.Maps;

/// <summary>
///     A cell in a map.
/// </summary>
public class MapCell
{
    /// <summary>
    ///     The unique ID of the containing map.
    /// </summary>
    public required long MapId { get; init; }

    /// <summary>
    ///     The unique number of the cell in the map.
    /// </summary>
    public required int CellNumber { get; init; }

    /// <summary>
    ///     The elevation of the cell in the map.
    /// </summary>
    public required int Floor { get; init; }

    /// <summary>
    ///     The move zone of the cell.
    /// </summary>
    public required int MoveZone { get; init; }

    /// <summary>
    ///     The linked zone of the cell.
    /// </summary>
    public required int LinkedZone { get; init; }

    /// <summary>
    ///     The speed modifier of the cell.
    /// </summary>
    public required int Speed { get; init; }

    /// <summary>
    ///     Can players see through the cell.
    /// </summary>
    public required bool Los { get; init; }

    /// <summary>
    ///     Is the cell visible.
    /// </summary>
    public required bool Visible { get; init; }

    /// <summary>
    ///     Is the cell non-walkable during fight.
    /// </summary>
    public required bool NonWalkableDuringFight { get; init; }

    /// <summary>
    ///     Is the cell non-walkable during RP.
    /// </summary>
    public required bool NonWalkableDuringRp { get; init; }

    /// <summary>
    ///     Is the cell a valid haven bag cell.
    /// </summary>
    public required bool HavenbagCell { get; init; }
}
