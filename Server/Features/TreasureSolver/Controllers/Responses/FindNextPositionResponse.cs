using System.ComponentModel.DataAnnotations;
using DBI.DataCenter.Structured.Models.Maps;

namespace DBI.Server.Features.TreasureSolver.Controllers.Responses;

/// <summary>
///     The map containing the next clue.
/// </summary>
public class FindNextPositionResponse
{
    /// <summary>
    ///     Has the next map been found.
    /// </summary>
    [Required]
    public required bool Found { get; init; }

    /// <summary>
    ///     The coordinates of the next map, if it has been found.
    /// </summary>
    public required Position? MapPosition { get; init; }
}
