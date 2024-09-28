using System.ComponentModel.DataAnnotations;
using Server.Common.Models;

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
    ///     The unique ID of the next map, if it has been found.
    /// </summary>
    public required long? MapId { get; init; }

    /// <summary>
    ///     The coordinates of the next map, if it has been found.
    /// </summary>
    public required Position? MapPosition { get; init; }
}
