using System.ComponentModel.DataAnnotations;
using DBI.DataCenter.Structured.Models.Maps;
using DBI.Server.Features.PathFinder.Controllers.Requests;

namespace DBI.Server.Features.TreasureSolver.Controllers.Requests;

/// <summary>
///     Search for the next clue in a treasure hunt.
/// </summary>
public class FindNextClueRequest
{
    /// <summary>
    ///     The request for the start node.
    /// </summary>
    [Required]
    public required FindNodeRequest Start { get; set; }

    /// <summary>
    ///     The direction in which to look for.
    /// </summary>
    [Required]
    public Direction Direction { get; set; }

    /// <summary>
    ///     The clue to look for.
    /// </summary>
    [Required]
    public int ClueId { get; set; }
}
