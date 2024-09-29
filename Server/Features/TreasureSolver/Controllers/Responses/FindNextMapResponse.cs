using System.ComponentModel.DataAnnotations;
using Server.Features.DataCenter.Models.Maps;

namespace Server.Features.TreasureSolver.Controllers.Responses;

/// <summary>
///     The map containing the next clue.
/// </summary>
public class FindNextMapResponse
{
    /// <summary>
    ///     Has the next map been found.
    /// </summary>
    [Required]
    public required bool Found { get; init; }

    /// <summary>
    ///     The map in which the clue has been found, if any.
    /// </summary>
    public MapNodeWithPosition? Map { get; init; }

    /// <summary>
    ///     The number of maps to traverse to reach the destination.
    /// </summary>
    public int? Distance { get; init; }
}
