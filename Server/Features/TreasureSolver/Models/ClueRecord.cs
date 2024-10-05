namespace DBI.Server.Features.TreasureSolver.Models;

/// <summary>
///     A record that a clue has been found (or not found) in a specific map.
/// </summary>
public class ClueRecord
{
    /// <summary>
    ///     The date at which the record has been created.
    /// </summary>
    public DateTime RecordDate { get; set; }

    /// <summary>
    ///     The unique ID of the map where the clue has been found (or not found).
    /// </summary>
    public long MapId { get; set; }

    /// <summary>
    ///     The unique ID of the clue that has been found (or not found).
    /// </summary>
    public int ClueId { get; set; }

    /// <summary>
    ///     Has the clue been found at the given position, or not found at the given position.
    /// </summary>
    public bool Found { get; set; }
}
